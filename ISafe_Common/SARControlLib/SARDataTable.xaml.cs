using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using SARControlLib;

namespace SARControlLib
{
	/// <summary>
	/// 数据表格
	/// </summary>
	public partial class SARDataTable : UserControl
	{
		public SARDataTable()
		{
            this.InitializeComponent();
		}

        ///// <summary>
        ///// 绑定的数据源，实例程序如下
        //public class TestBean
        //{
        //    public string Header1;
        //    public string Header2;
        //    public string Header3;
        //    public string Header4;

        //    public TestBean(string s1,string s2,string s3,string s4)
        //    {
        //        Header1 = s1;
        //        Header2 = s2;
        //        Header3 = s3;
        //        Header4 = s4;
        //    }
        //}
        
            
        /// </summary>
        public IEnumerable SARItemsSource
        {
            set
            {
                this.dataGrid.ItemsSource = value;
            }
            get
            {
                return this.dataGrid.ItemsSource;
            }
        }

        /// <summary>
        /// 内部DataGrid，一般情况下，不需要使用
        /// </summary>
        public DataGrid SARnterDataGrid
        {
            get
            {
                return this.dataGrid;                   
            }
        }
		
		
        /// <summary>
        /// 设置表格的标题
        /// </summary>
		public string DataGridTitle
		{
			set{ this.Title.Text = value; }
			get{ return  this.Title.Text; }
		}

        /// <summary>
        /// 返回选中的行
        /// </summary>
      
        public string Getres()
        {
            return ((this.dataGrid.Items[0]) as TextBlock).Text;
        }
	}

}