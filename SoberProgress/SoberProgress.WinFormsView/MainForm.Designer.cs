namespace SoberProgress.WinFormsView
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvLeaderboard = new DataGridView();
            txtSurname = new TextBox();
            txtName = new TextBox();
            txtPatronymic = new TextBox();
            dtpLastDrink = new DateTimePicker();
            btnAdd = new Button();
            btnCode = new Button();
            btnRelapse = new Button();
            btnDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvLeaderboard).BeginInit();
            SuspendLayout();
            // 
            // dgvLeaderboard
            // 
            dgvLeaderboard.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLeaderboard.Location = new Point(509, 23);
            dgvLeaderboard.Name = "dgvLeaderboard";
            dgvLeaderboard.Size = new Size(432, 384);
            dgvLeaderboard.TabIndex = 0;
            // 
            // txtSurname
            // 
            txtSurname.Location = new Point(72, 108);
            txtSurname.Name = "txtSurname";
            txtSurname.Size = new Size(100, 23);
            txtSurname.TabIndex = 1;
            // 
            // txtName
            // 
            txtName.Location = new Point(191, 108);
            txtName.Name = "txtName";
            txtName.Size = new Size(100, 23);
            txtName.TabIndex = 2;
            // 
            // txtPatronymic
            // 
            txtPatronymic.Location = new Point(311, 108);
            txtPatronymic.Name = "txtPatronymic";
            txtPatronymic.Size = new Size(100, 23);
            txtPatronymic.TabIndex = 3;
            // 
            // dtpLastDrink
            // 
            dtpLastDrink.Location = new Point(145, 159);
            dtpLastDrink.Name = "dtpLastDrink";
            dtpLastDrink.Size = new Size(200, 23);
            dtpLastDrink.TabIndex = 4;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(157, 38);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(167, 47);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "Добавить Алкаша";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnCode
            // 
            btnCode.Location = new Point(38, 249);
            btnCode.Name = "btnCode";
            btnCode.Size = new Size(167, 47);
            btnCode.TabIndex = 6;
            btnCode.Text = "Закодировать";
            btnCode.UseVisualStyleBackColor = true;
            btnCode.Click += btnCode_Click;
            // 
            // btnRelapse
            // 
            btnRelapse.Location = new Point(268, 249);
            btnRelapse.Name = "btnRelapse";
            btnRelapse.Size = new Size(167, 47);
            btnRelapse.TabIndex = 7;
            btnRelapse.Text = "Отметить Срыв";
            btnRelapse.UseVisualStyleBackColor = true;
            btnRelapse.Click += btnRelapse_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(157, 336);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(167, 47);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Удалить алкаша";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 450);
            Controls.Add(btnDelete);
            Controls.Add(btnRelapse);
            Controls.Add(btnCode);
            Controls.Add(btnAdd);
            Controls.Add(dtpLastDrink);
            Controls.Add(txtPatronymic);
            Controls.Add(txtName);
            Controls.Add(txtSurname);
            Controls.Add(dgvLeaderboard);
            Name = "MainForm";
            Text = "Алкашный тир лист";
            ((System.ComponentModel.ISupportInitialize)dgvLeaderboard).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvLeaderboard;
        private TextBox txtSurname;
        private TextBox txtName;
        private TextBox txtPatronymic;
        private DateTimePicker dtpLastDrink;
        private Button btnAdd;
        private Button btnCode;
        private Button btnRelapse;
        private Button btnDelete;
    }
}
