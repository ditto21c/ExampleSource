using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace LoadExcel
{
    public partial class Form1 : Form
    {
        enum eCheckBox
        {
            eCheckBox_Character = 0,
            eCheckBox_Map,
            eCheckBox_Skill,
            eCheckBox_Item,
            eCheckBox_Stage,
            eCheckBox_All,
        };
        
        public Form1()
        {
            InitializeComponent();
        }
        
        // 툴 저장 버튼 클릭 [4/20/2012 ditto1]
        private void button1_Click(object sender, EventArgs e)
        {
            // 파씽 시작
            m_AllItemProgress.Value = 0;
            m_AllItemProgress.Maximum = GetSelectItemCount();
            m_AllItemProgress.Step = 1;

            for (int i = 0; i < m_checkedListBox.Items.Count-1; ++i)
            {
                if(m_checkedListBox.GetItemChecked(i))
                {
                    switch (i)
                    {
                        case (int)eCheckBox.eCheckBox_Character:
                            CCharacterDB CharDB = new CCharacterDB();
                            CharDB.LoadExcel();
                            CharDB.Save();
                           // CharDB.Load();
                            break;
                        case (int)eCheckBox.eCheckBox_Map:
                            break;
                        case (int)eCheckBox.eCheckBox_Skill:
                            CSkillDB SkillDB = new CSkillDB();
                            SkillDB.LoadExcel();
                            SkillDB.Save();
                            break;
                        case (int)eCheckBox.eCheckBox_Item:
                            CItemDB ItemDB = new CItemDB();
                            ItemDB.LoadExcel();
                            ItemDB.Save();
                            break;
                        case (int)eCheckBox.eCheckBox_Stage:
                            CStageDB kStateDB = new CStageDB();
                            kStateDB.LoadExcel();
                            kStateDB.Save();
                            break;
                    }

                    m_AllItemProgress.Value += m_AllItemProgress.Step;
                }
                
            }
        }

        // 전체 아이템 체크 상태에 따른 다른 아이템들 체크 선택 및 해제[4/21/2012 JK]
        private void m_checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelectIndex = m_checkedListBox.SelectedIndex;
            if (0 <= iSelectIndex
                && iSelectIndex <= (int)eCheckBox.eCheckBox_All)
            {
                if ((int)eCheckBox.eCheckBox_All == iSelectIndex)
                {
                    if (m_checkedListBox.GetItemChecked((int)eCheckBox.eCheckBox_All))
                    {
                        for (int i = 0; i < m_checkedListBox.Items.Count - 1; ++i)
                        {
                            m_checkedListBox.SetItemChecked(i, true);
                        }

                    }
                    else
                    {
                        for (int i = 0; i < m_checkedListBox.Items.Count - 1; ++i)
                        {
                            m_checkedListBox.SetItemChecked(i, false);
                        }
                    }
                }
                else
                {

                    if (m_checkedListBox.GetItemChecked(iSelectIndex))
                    {
                        m_checkedListBox.SetItemChecked(iSelectIndex, true);
                    }
                    else
                    {
                        m_checkedListBox.SetItemChecked(iSelectIndex, false);
                    }
                }
            }
            
           
        }

        // 체크된 아이템 갯수 리턴 [4/21/2012 JK]
        private int GetSelectItemCount()
        {
            int iCount = 0;
            for (int i = 0; i < m_checkedListBox.Items.Count-1; ++i)
            {
                // 시작
                if (m_checkedListBox.GetItemChecked(i))
                {
                    iCount++;
                }
            }
            return iCount;
        }
    
    }
}
