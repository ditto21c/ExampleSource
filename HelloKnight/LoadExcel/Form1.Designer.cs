namespace LoadExcel
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.m_checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.m_AllItemProgress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(205, 231);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "저장";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // m_checkedListBox
            // 
            this.m_checkedListBox.CheckOnClick = true;
            this.m_checkedListBox.FormattingEnabled = true;
            this.m_checkedListBox.Items.AddRange(new object[] {
            "캐릭터",
            "맵",
            "스킬",
            "아이템",
            "스테이지",
            "전체"});
            this.m_checkedListBox.Location = new System.Drawing.Point(12, 12);
            this.m_checkedListBox.Name = "m_checkedListBox";
            this.m_checkedListBox.Size = new System.Drawing.Size(268, 180);
            this.m_checkedListBox.TabIndex = 3;
            this.m_checkedListBox.SelectedIndexChanged += new System.EventHandler(this.m_checkedListBox_SelectedIndexChanged);
            // 
            // m_AllItemProgress
            // 
            this.m_AllItemProgress.Location = new System.Drawing.Point(12, 196);
            this.m_AllItemProgress.Name = "m_AllItemProgress";
            this.m_AllItemProgress.Size = new System.Drawing.Size(268, 23);
            this.m_AllItemProgress.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.m_AllItemProgress);
            this.Controls.Add(this.m_checkedListBox);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "ExcelDataLoad(Ver 1.0)";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckedListBox m_checkedListBox;
        private System.Windows.Forms.ProgressBar m_AllItemProgress;
    }
}

