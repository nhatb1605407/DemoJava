namespace EmployeeManager
{
    partial class delete_data
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtdelete = new System.Windows.Forms.Label();
            this.bntyes = new System.Windows.Forms.Button();
            this.bntno = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtdelete
            // 
            this.txtdelete.AutoSize = true;
            this.txtdelete.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtdelete.Location = new System.Drawing.Point(35, 40);
            this.txtdelete.Name = "txtdelete";
            this.txtdelete.Size = new System.Drawing.Size(311, 42);
            this.txtdelete.TabIndex = 0;
            this.txtdelete.Text = "Bạn có chắt chắn muốn xóa những mục \r\n                  đã được chọn ?\r\n";
            // 
            // bntyes
            // 
            this.bntyes.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.bntyes.Location = new System.Drawing.Point(75, 104);
            this.bntyes.Name = "bntyes";
            this.bntyes.Size = new System.Drawing.Size(78, 37);
            this.bntyes.TabIndex = 1;
            this.bntyes.Text = "Đồng ý";
            this.bntyes.UseVisualStyleBackColor = true;
            // 
            // bntno
            // 
            this.bntno.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.bntno.Location = new System.Drawing.Point(229, 104);
            this.bntno.Name = "bntno";
            this.bntno.Size = new System.Drawing.Size(83, 37);
            this.bntno.TabIndex = 2;
            this.bntno.Text = "Hủy";
            this.bntno.UseVisualStyleBackColor = true;
            // 
            // delete_data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 171);
            this.Controls.Add(this.bntno);
            this.Controls.Add(this.bntyes);
            this.Controls.Add(this.txtdelete);
            this.Name = "delete_data";
            this.Text = "Delete";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label txtdelete;
        private System.Windows.Forms.Button bntyes;
        private System.Windows.Forms.Button bntno;
    }
}