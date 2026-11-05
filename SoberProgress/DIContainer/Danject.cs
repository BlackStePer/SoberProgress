using System.Collections.Concurrent;
using System.Reflection;

namespace DIContainer
{
    /// <summary>
    /// Жизненный цикл создаваемых объектов
    /// </summary>
    public enum Lifetime
    {
        Transient,
        Singleton
    }

    /// <summary>
    /// DI контейнер
    /// </summary>
    public class Danject
    {
        private class Binding
        {
            public Type ImplementationType { get; set; }
            public Lifetime Lifetime { get; set; }
            public object SingletonInstance { get; set; }
        }

        private readonly ConcurrentDictionary<Type, Binding> _bindings = new ConcurrentDictionary<Type, Binding>();
        private readonly HashSet<Type> _instantiatingTypes = new HashSet<Type>();


        /// <summary>
        /// Зарегистрировать сопоставление интерфейса и его конкретной реализации
        /// </summary>
        /// <typeparam name="TInterface">Базовый тип</typeparam>
        /// <typeparam name="TImplementation">Реализация</typeparam>
        /// <param name="lifetime">Время жизни</param>
        public void Register<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient) where TImplementation : class, TInterface
        {
            _bindings[typeof(TInterface)] = new Binding
            {
                ImplementationType = typeof(TImplementation),
                Lifetime = lifetime
            };
        }

        /// <summary>
        /// Получить готовый объект с автоматически прокинутыми зависимостями
        /// </summary>
        /// <typeparam name="T">Тип запрашиваемого объекта</typeparam>
        /// <returns>Экземпляр запрашиваемого типа</returns>
        public T Resolve<T>() => (T)Resolve(typeof(T));

        /// <summary>
        /// Разрешает зависимость по указанному типу данных.
        /// </summary>
        /// <param name="type">Тип запрашиваемого объекта.</param>
        /// <returns>Созданный экземпляр объекта.</returns>
        /// <exception cref="InvalidOperationException">Генерируется при обнаружении круговой зависимости или попытке разрешить незарегистрированный абстрактный тип.</exception>
        public object Resolve(Type type)
        {
            if (_instantiatingTypes.Contains(type))
                throw new InvalidOperationException($"Обнаружена круговая зависимость для типа: {type.FullName}");

            if (!_bindings.TryGetValue(type, out var binding))
            {
                if (type.IsInterface || type.IsAbstract)
                    throw new InvalidOperationException($"Ошибка DI: Интерфейс или абстрактный тип {type.FullName} не зарегистрирован в контейнере.");

                binding = new Binding { ImplementationType = type, Lifetime = Lifetime.Transient };
            }

            if (binding.Lifetime == Lifetime.Singleton && binding.SingletonInstance != null)
                return binding.SingletonInstance;

            _instantiatingTypes.Add(type);
            try
            {
                object instance = CreateInstance(binding.ImplementationType);

                if (binding.Lifetime == Lifetime.Singleton)
                    binding.SingletonInstance = instance;

                InjectDependencies(instance);

                return instance;
            }
            finally
            {
                _instantiatingTypes.Remove(type);
            }
        }

        private object CreateInstance(Type type)
        {
            var constructor = type.GetConstructors()
                .FirstOrDefault(c => c.GetCustomAttribute<InjectAttribute>() != null)
                ?? type.GetConstructors().FirstOrDefault();

            if (constructor == null)
                return Activator.CreateInstance(type);

            var parameters = constructor.GetParameters();
            var args = parameters.Select(p => Resolve(p.ParameterType)).ToArray();

            return constructor.Invoke(args);
        }

        private void InjectDependencies(object instance)
        {
            if (instance == null) return;
            var type = instance.GetType();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => p.GetCustomAttribute<InjectAttribute>() != null);

            foreach (var prop in properties)
            {
                if (prop.CanWrite)
                {
                    var dependency = Resolve(prop.PropertyType);
                    prop.SetValue(instance, dependency);
                }
            }

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.GetCustomAttribute<InjectAttribute>() != null);

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                var args = parameters.Select(p => Resolve(p.ParameterType)).ToArray();
                method.Invoke(instance, args);
            }
        }
    }
}
