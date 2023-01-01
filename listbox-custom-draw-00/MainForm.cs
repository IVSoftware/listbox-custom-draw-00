﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace listbox_custom_draw_00
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            listBox1.DisplayMember = nameof(MyListBoxItem.Message);
            listBox1.DataSource = MyItems;
            listBox1.DrawItem += onDrawItem;
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.SelectedIndexChanged += (sender, e) => listBox1.Refresh();
        }

        private void onDrawItem(object sender, DrawItemEventArgs e)
        {
            var myItem = MyItems[e.Index];

            if(listBox1.SelectedItems.Contains(myItem))
            {
                using (var backgroundBrush = new SolidBrush(myItem.ItemColor))
                {
                    e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                }

                using (var textBrush = new SolidBrush(Color.White))
                {
                    e.Graphics.DrawString(myItem.Message, listBox1.Font, textBrush, e.Bounds);
                }
            }
            else
            {
                using (var backgroundBrush = new SolidBrush(SystemColors.Window))
                {
                    e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                }
                using (var textBrush = new SolidBrush(myItem.ItemColor))
                {
                    e.Graphics.DrawString(myItem.Message, listBox1.Font, textBrush, e.Bounds);
                }
            }
        }

        BindingList<MyListBoxItem> MyItems { get; } = new BindingList<MyListBoxItem>
        {
            new MyListBoxItem
            {
                Message = "Validated data successfully",
                ItemColor= Color.Green,
            },
            new MyListBoxItem
            {
                Message = "Failed to validate data",
                ItemColor= Color.Red,
            },
        };
    }
    public class MyListBoxItem
    { 
        public Color ItemColor { get; set; }
        public string Message { get; set; }
        public override string ToString() => Message;
    }
    //const string mockFileContents = @"";
}
