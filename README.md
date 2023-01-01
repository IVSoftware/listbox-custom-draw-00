Your question is **How to color items in listBox in different colors?** and the "Y" of what might be considered an [X-Y Problem](https://meta.stackexchange.com/a/66378) is that you get an exception when you try to Add an item inline. After carefully reading your code, something that would make a big difference would be using the `MyListBoxItem` type consistently in both in your `DataSource` and your Json serialization and deserialization, and then appending the data source when you wish to Add an item inline.

[![add-inline][1]][1]

***
**Datasource**

    BindingList<MyListBoxItem> MyItems { get; } = new BindingList<MyListBoxItem>();

**Example of MyItems in Json-Serialized form in the disk file**

    [
      {
        "ItemColor": "Blue",
        "Message": "Blue Item"
      },
      {
        "ItemColor": "Green",
        "Message": "Green Item"
      },
      {
        "ItemColor": "Red",
        "Message": "Red Item"
      }
    ]



***
**Main Form initialization for listBox1 drawing code**

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

            // Tests
            buttonTest.Click += onButtonTest;
            buttonReadJson.Click += onButtonReadJson;
        }
        private void onDrawItem(object sender, DrawItemEventArgs e)
        {
            if((e.Index == -1) || (e.Index >= MyItems.Count))
            {                
                e.DrawBackground();
            }
            else
            {
                var myItem = MyItems[e.Index];
                if (listBox1.SelectedItems.Contains(myItem))
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
        }
        BindingList<MyListBoxItem> MyItems { get; } = new BindingList<MyListBoxItem>();
        .
        .
        .
    }

***
**Example of adding items**

    private void onButtonTest(object sender, EventArgs e)
    {
        MyItems.Clear();
        MyItems.Add(new MyListBoxItem
        {
            Message = "Validated data successfully",
            ItemColor = Color.Green,
        });
        MyItems.Add(new MyListBoxItem
        {
            Message = "Failed to validate data",
            ItemColor = Color.Red,
        });
    }

 ***
 **Example of deserializing file**

[![json deserialize][2]][2]

    private void onButtonReadJson(object sender, EventArgs e)
    {
        MyItems.Clear();
        foreach (
            var myItem 
            in JsonConvert.DeserializeObject<List<MyListBoxItem>>(mockFileContents))
        {
            MyItems.Add(myItem);
        }
    }

    const string mockFileContents = 
    @"[
      {
        ""ItemColor"": ""Blue"",
        ""Message"": ""Blue Item""
      },
      {
        ""ItemColor"": ""Green"",
        ""Message"": ""Green Item""
      },
      {
        ""ItemColor"": ""Red"",
        ""Message"": ""Red Item""
      }
    ]";


  [1]: https://i.stack.imgur.com/ZptG8.png
  [2]: https://i.stack.imgur.com/4H187.png