
namespace TinyCompiler
{
    partial class TinyCompilerForm
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
            this.tokens_dgv = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.parserErrors_t = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ParseTree = new System.Windows.Forms.TreeView();
            this.Clear_btn = new System.Windows.Forms.Button();
            this.Compile_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tinyCode_t = new System.Windows.Forms.TextBox();
            this.errorList_t = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tokens_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // tokens_dgv
            // 
            this.tokens_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tokens_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.tokens_dgv.Location = new System.Drawing.Point(369, 27);
            this.tokens_dgv.Margin = new System.Windows.Forms.Padding(2);
            this.tokens_dgv.Name = "tokens_dgv";
            this.tokens_dgv.RowHeadersWidth = 51;
            this.tokens_dgv.RowTemplate.Height = 24;
            this.tokens_dgv.Size = new System.Drawing.Size(302, 226);
            this.tokens_dgv.TabIndex = 33;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Lexeme";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 125;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Token Class";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 125;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(366, 300);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "Parser Errors";
            // 
            // parserErrors_t
            // 
            this.parserErrors_t.Location = new System.Drawing.Point(369, 315);
            this.parserErrors_t.Multiline = true;
            this.parserErrors_t.Name = "parserErrors_t";
            this.parserErrors_t.ReadOnly = true;
            this.parserErrors_t.Size = new System.Drawing.Size(302, 261);
            this.parserErrors_t.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(684, 8);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Tree View";
            // 
            // ParseTree
            // 
            this.ParseTree.Location = new System.Drawing.Point(686, 24);
            this.ParseTree.Name = "ParseTree";
            this.ParseTree.Size = new System.Drawing.Size(484, 552);
            this.ParseTree.TabIndex = 36;
            // 
            // Clear_btn
            // 
            this.Clear_btn.Location = new System.Drawing.Point(463, 257);
            this.Clear_btn.Margin = new System.Windows.Forms.Padding(2);
            this.Clear_btn.Name = "Clear_btn";
            this.Clear_btn.Size = new System.Drawing.Size(100, 41);
            this.Clear_btn.TabIndex = 32;
            this.Clear_btn.Text = "Clear";
            this.Clear_btn.UseVisualStyleBackColor = true;
            this.Clear_btn.Click += new System.EventHandler(this.Clear_btn_Click);
            // 
            // Compile_btn
            // 
            this.Compile_btn.Location = new System.Drawing.Point(105, 257);
            this.Compile_btn.Margin = new System.Windows.Forms.Padding(2);
            this.Compile_btn.Name = "Compile_btn";
            this.Compile_btn.Size = new System.Drawing.Size(126, 41);
            this.Compile_btn.TabIndex = 31;
            this.Compile_btn.Text = "Compile !";
            this.Compile_btn.UseVisualStyleBackColor = true;
            this.Compile_btn.Click += new System.EventHandler(this.Compile_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Tiny Code";
            // 
            // tinyCode_t
            // 
            this.tinyCode_t.Location = new System.Drawing.Point(16, 27);
            this.tinyCode_t.Margin = new System.Windows.Forms.Padding(2);
            this.tinyCode_t.Multiline = true;
            this.tinyCode_t.Name = "tinyCode_t";
            this.tinyCode_t.Size = new System.Drawing.Size(335, 226);
            this.tinyCode_t.TabIndex = 29;
            // 
            // errorList_t
            // 
            this.errorList_t.Location = new System.Drawing.Point(12, 315);
            this.errorList_t.Margin = new System.Windows.Forms.Padding(2);
            this.errorList_t.Multiline = true;
            this.errorList_t.Name = "errorList_t";
            this.errorList_t.ReadOnly = true;
            this.errorList_t.Size = new System.Drawing.Size(339, 261);
            this.errorList_t.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 300);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Error List";
            // 
            // TinyCompilerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1183, 588);
            this.Controls.Add(this.tokens_dgv);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.parserErrors_t);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ParseTree);
            this.Controls.Add(this.Clear_btn);
            this.Controls.Add(this.Compile_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tinyCode_t);
            this.Controls.Add(this.errorList_t);
            this.Controls.Add(this.label2);
            this.Name = "TinyCompilerForm";
            this.Text = "Tiny Compiler";
            ((System.ComponentModel.ISupportInitialize)(this.tokens_dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView tokens_dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox parserErrors_t;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView ParseTree;
        private System.Windows.Forms.Button Clear_btn;
        private System.Windows.Forms.Button Compile_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tinyCode_t;
        private System.Windows.Forms.TextBox errorList_t;
        private System.Windows.Forms.Label label2;
    }
}

