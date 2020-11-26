using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace YchetTovarov
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Config config;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                config = new Config();
                Refresh();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        public void Refresh()
        {
            TovarList.Children.Clear();
            SkladList.Children.Clear();
            PostavList.Children.Clear();
            ZakazList.Children.Clear();
            #region tovar
            foreach (Product product in config.list.AllProducts)
            {
                UserControl1 userControl1 = new UserControl1();
                userControl1.NameLabel.Content = product.Name;
                userControl1.DescLabel.Text = product.Desc;
                userControl1.Trash.PreviewMouseDown += delegate
                {
                    if (config.list.AllstroredProducts.Where(x => x.product == product).Count() > 0)
                    {
                        MessageBox.Show("На ваших полочках или в заказах престутсвует данный товар!");
                    }
                    else
                    {
                        config.list.AllProducts.Remove(product);
                        config.Save();
                        Refresh();
                    }
                };
                userControl1.Edit.PreviewMouseDown += delegate
                {
                    if (addForm != null)
                        addForm.Close();
                    addForm = new AddForm();
                    addForm.Show();
                    addForm.nametext.Text = product.Name; addForm.desctext.Text = product.Desc;
                    addForm.Sumbit.Click += delegate
                    {
                        product.Name = addForm.nametext.Text; product.Desc = addForm.desctext.Text;
                        if (addForm != null)
                            addForm.Close();
                        config.Save();
                        Refresh();
                    }
                    ;

                };
                TovarList.Children.Add(userControl1);
            }
            #endregion

            #region Postav
            foreach (Postavshick product in config.list.AllPostavshicks)
            {
                UserControl1 userControl1 = new UserControl1();
                userControl1.NameLabel.Content = product.name;
                userControl1.DescLabel.Text = product.tel + Environment.NewLine + product.adress;
                userControl1.Trash.PreviewMouseDown += delegate
                {
                    if (config.list.AllstroredProducts.Where(x => x.Postavshick == product).Count() > 0)
                    {
                        MessageBox.Show("Ваш товар привязан к данному поставщику!");
                    }
                    else
                    {
                        config.list.AllPostavshicks.Remove(product);
                        config.Save();
                        Refresh();
                    }
                };
                userControl1.Edit.PreviewMouseDown += delegate
                {
                    if (addForm3 != null)
                        addForm3.Close();
                    addForm3 = new AddForm3();
                    addForm3.Show();
                    addForm3.nameL.Text = product.name;
                    addForm3.TelL.Text = product.tel;
                    addForm3.adressT.Text = product.adress;
                    addForm3.Sumbit.Click += delegate
                    {
                        product.adress = addForm3.adressT.Text; product.tel = addForm3.TelL.Text;
                        product.name = addForm3.nameL.Text;
                        if (addForm3 != null)
                            addForm3.Close();
                        config.Save();
                        Refresh();
                    }
                    ;

                };
                PostavList.Children.Add(userControl1);
            }
            #endregion
            #region Store
            foreach (StroredProduct product in config.list.AllstroredProducts)
            {
                UserControl2 userControl1 = new UserControl2();
                userControl1.NameLabel.Content = product.product.Name;
                userControl1.Kolichesvo.Content = product.count;
                userControl1.Polochka.Content = product.polka;
                userControl1.post.Content = product.Postavshick.name;
                userControl1.Trash.PreviewMouseDown += delegate
                {
                    config.list.AllstroredProducts.Remove(product);
                    config.Save();
                    Refresh();
                };
                userControl1.Edit.PreviewMouseDown += delegate
                {
                    if (addForm2 != null)
                        addForm2.Close();
                    addForm2 = new AddForm2();
                    addForm2.Show();
                    addForm2.poltext.Text = product.polka.ToString();
                    addForm2.koltext.Text = product.count.ToString();
                    foreach (UserControl1 user in TovarList.Children.Cast<UserControl1>().ToArray())
                        addForm2.TovarList.Items.Add(new ListBoxItem() { Content = user.NameLabel.Content });
                    foreach (UserControl1 user in PostavList.Children.Cast<UserControl1>().ToArray())
                        addForm2.PostavshickList.Items.Add(new ListBoxItem() { Content = user.NameLabel.Content });
                    addForm2.TovarList.SelectedItem = addForm2.TovarList.Items.Cast<ListBoxItem>().Where(x => x.Content.ToString() == product.product.Name).First();
                    addForm2.PostavshickList.SelectedItem = addForm2.PostavshickList.Items.Cast<ListBoxItem>().Where(x => x.Content.ToString() == product.Postavshick.name).First();
                    addForm2.Sumbit.Click += delegate
                    {
                        product.polka = Convert.ToInt32(addForm2.poltext.Text);
                        product.count = Convert.ToInt32(addForm2.koltext.Text);
                        product.product = config.list.AllProducts.Where(x => x.Name == ((ListBoxItem)addForm2.TovarList.SelectedItem).Content.ToString()).First();
                        product.Postavshick = config.list.AllPostavshicks.Where(x => x.name == ((ListBoxItem)addForm2.PostavshickList.SelectedItem).Content.ToString()).First();
                        if (addForm2 != null)
                            addForm2.Close();
                        config.Save();
                        Refresh();
                    }
                    ;

                };
                SkladList.Children.Add(userControl1);
            }
            #endregion
            #region Zakaz
            foreach (Zakaz product in config.list.AllZakazs)
            {
                UserControl2 userControl1 = new UserControl2();
                userControl1.NameLabel.Content = product.stroredProduct.Name;
                userControl1.Kolichesvo.Content = product.Count;
                userControl1.PolPanel.Visibility = Visibility.Collapsed;
                userControl1.post.Content = product.postavshick.name;
                userControl1.Trash.PreviewMouseDown += delegate
                {
                    config.list.AllZakazs.Remove(product);
                    config.Save();
                    Refresh();
                };
                userControl1.Edit.PreviewMouseDown += delegate
                {
                    if (addForm4 != null)
                        addForm4.Close();
                    addForm4 = new AddForm4();
                    addForm4.Show();
                   
                    addForm4.koltext.Text = product.Count.ToString();
                    foreach (UserControl1 user in TovarList.Children.Cast<UserControl1>().ToArray())
                        addForm4.TovarList.Items.Add(new ListBoxItem() { Content = user.NameLabel.Content });
                    foreach (UserControl1 user in PostavList.Children.Cast<UserControl1>().ToArray())
                        addForm4.PostavshickList.Items.Add(new ListBoxItem() { Content = user.NameLabel.Content });
                    addForm4.TovarList.SelectedItem = addForm4.TovarList.Items.Cast<ListBoxItem>().Where(x => x.Content.ToString() == product.stroredProduct.Name).First();
                    addForm4.PostavshickList.SelectedItem = addForm4.PostavshickList.Items.Cast<ListBoxItem>().Where(x => x.Content.ToString() == product.postavshick.name).First();
                    addForm4.Sumbit.Click += delegate
                    {
                        product.Count = Convert.ToInt32(addForm2.koltext.Text);
                       
                        product.stroredProduct = config.list.AllProducts.Where(x => x.Name == ((ListBoxItem)addForm4.TovarList.SelectedItem).Content.ToString()).First();
                        product.postavshick = config.list.AllPostavshicks.Where(x => x.name == ((ListBoxItem)addForm4.PostavshickList.SelectedItem).Content.ToString()).First();
                        if (addForm4 != null)
                            addForm4.Close();
                        config.Save();
                        Refresh();
                    }
                    ;

                };
                ZakazList.Children.Add(userControl1);
            }
            #endregion
        }


        AddForm addForm;
        AddForm2 addForm2;
        AddForm3 addForm3;
        AddForm4 addForm4;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (addForm != null)
                addForm.Close();
            addForm = new AddForm();
            addForm.Show();
            addForm.Sumbit.Click += Sumbit_Click;
        }

        private void Sumbit_Click(object sender, RoutedEventArgs e)
        {
            string name = addForm.nametext.Text;
            string desc = addForm.desctext.Text;
            if (name.Length == 0 || desc.Length == 0)
            {
                MessageBox.Show(
                    "Нужно указать товар и его описание",
                    "Ошибка валидации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }
            Product product = new Product() { Name = name, Desc = desc };
            config.list.AllProducts.Add(product);
            config.Save();
            Refresh();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (addForm != null)
                addForm.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(config.list.AllProducts.Count==0)
            {
                MessageBox.Show("У вас нет ни одного товара!");
                return;
            }
            if (config.list.AllPostavshicks.Count == 0)
            {
                MessageBox.Show("У вас нет ни одного поставщика!");
                return;
            }
            if (addForm2 != null)
                addForm2.Close();
            addForm2 = new AddForm2();
            addForm2.Show();
            foreach (UserControl1 user in TovarList.Children.Cast<UserControl1>().ToArray())
                addForm2.TovarList.Items.Add(new ListBoxItem() { Content = user.NameLabel.Content });
            foreach (UserControl1 user in PostavList.Children.Cast<UserControl1>().ToArray())
                addForm2.PostavshickList.Items.Add(new ListBoxItem() { Content = user.NameLabel.Content });
            addForm2.Sumbit.Click += Sumbit_Click1; ;
        }

        private void Sumbit_Click1(object sender, RoutedEventArgs e)
        {
            if (addForm2.poltext.Text.Length == 0 || addForm2.koltext.Text.Length == 0 || addForm2.PostavshickList.SelectedItem == null || addForm2.TovarList.SelectedItem == null)
            {
                MessageBox.Show(
                    "Нужно указать полку, товар, его количество и поставщика",
                    "Ошибка валидации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (!Int32.TryParse(addForm2.poltext.Text, out int shelve))
            {
                MessageBox.Show("Полка должна быть характеризована номером!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Int32.TryParse(addForm2.koltext.Text, out int amount))
            {
                MessageBox.Show("Количество должно быть характеризовано числом!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(amount < 1)
            {
                MessageBox.Show("Вы не можете добавить количество товара меньше 1!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            StroredProduct product = new StroredProduct() { polka = Convert.ToInt32(shelve), count = Convert.ToInt32(amount),
                product = config.list.AllProducts.Where(x => x.Name == ((ListBoxItem)addForm2.TovarList.SelectedItem).Content.ToString()).First(),
                Postavshick = config.list.AllPostavshicks.Where(x => x.name == ((ListBoxItem)addForm2.PostavshickList.SelectedItem).Content.ToString()).First()

            };
            config.list.AllstroredProducts.Add(product);
            config.Save();
            Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (addForm3 != null)
                addForm3.Close();
            addForm3 = new AddForm3();
            addForm3.Show();
            addForm3.Sumbit.Click += Sumbit_Click2;
        }

        private void Sumbit_Click2(object sender, RoutedEventArgs e)
        {
            string phone = addForm3.TelL.Text;
            string address = addForm3.adressT.Text;
            string name = addForm3.nameL.Text;

            if (phone.Length == 0 || address.Length == 0 || name.Length == 0)
            {
                MessageBox.Show(
                    "Нужно указать телефон, адрес и наименование поставщика",
                    "Ошибка валидации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (phone[0] == '+')
            {
                phone = phone.Substring(1);
            }
            phone = Regex.Replace(phone, @"[-\(\) ]+", "");

            Regex phone_mask = new Regex(@"^[78][89][0-9]{9}$");
            if(!phone_mask.IsMatch(phone))
            {
                MessageBox.Show(
                    "Укажите валидный телефон, состоящий из 11 цифр. Можно также использовать пробел, скобки, дефис", 
                    "Ошибка валидации",
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning
                );
                return;
            }            

            Postavshick postavshick = new Postavshick()
            {
                adress = address,
                name = name,
                tel = phone
            };
            config.list.AllPostavshicks.Add(postavshick);
            config.Save();
            Refresh();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (config.list.AllProducts.Count == 0)
            {
                MessageBox.Show("У вас нет ни одного товара!");
                return;
            }
            if (config.list.AllPostavshicks.Count == 0)
            {
                MessageBox.Show("У вас нет ни одного поставщика!");
                return;
            }
            if (addForm4 != null)
                addForm4.Close();
            addForm4 = new AddForm4();
            addForm4.Show();
            foreach (UserControl1 user in TovarList.Children.Cast<UserControl1>().ToArray())
                addForm4.TovarList.Items.Add(new ListBoxItem() { Content = user.NameLabel.Content });
            foreach (UserControl1 user in PostavList.Children.Cast<UserControl1>().ToArray())
                addForm4.PostavshickList.Items.Add(new ListBoxItem() { Content = user.NameLabel.Content });
            addForm4.Sumbit.Click += Sumbit_Click3;
        }

        private void Sumbit_Click3(object sender, RoutedEventArgs e)
        {
            if (addForm4.koltext.Text.Length == 0 || addForm4.TovarList.SelectedItem == null || addForm4.PostavshickList.SelectedItem == null)
            {
                MessageBox.Show(
                    "Нужно указать товар, его количество и поставщика",
                    "Ошибка валидации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (!Int32.TryParse(addForm4.koltext.Text, out int count))
            {
                MessageBox.Show("Количество должно быть характеризовано номером!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Zakaz zakaz = new Zakaz() {
                Count = count,
                stroredProduct = config.list.AllProducts.Where(x => x.Name == ((ListBoxItem)addForm4.TovarList.SelectedItem).Content.ToString()).First(),
                postavshick = config.list.AllPostavshicks.Where(x => x.name == ((ListBoxItem)addForm4.PostavshickList.SelectedItem).Content.ToString()).First()
         };
            config.list.AllZakazs.Add(zakaz);
            config.Save();
            Refresh();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string pathDocument = AppDomain.CurrentDomain.BaseDirectory + "Отчет" + ".docx";

           
            DocX document = DocX.Create(pathDocument);


            Xceed.Document.NET.Paragraph paragraph = document.InsertParagraph();
           
            paragraph.Alignment = Alignment.center;

     
            paragraph.AppendLine("Отчет").Bold().Font("Times New Roman").FontSize(14);
       
            Xceed.Document.NET.Paragraph paragraph2 = document.InsertParagraph();
            paragraph2.Font("Times New Roman");
            paragraph2.FontSize(14);
            paragraph2.Alignment = Alignment.left;
            paragraph2.AppendLine("Список товаров: ").Font("Times New Roman").FontSize(14).Bold();
            paragraph2.AppendLine();
            foreach(Product product in config.list.AllProducts)
            {
                paragraph2.AppendLine("Название: "+product.Name).Font("Times New Roman").FontSize(14);
                paragraph2.AppendLine("Описание: " + product.Desc).Font("Times New Roman").FontSize(12);
                paragraph2.AppendLine();
            }
            paragraph2.AppendLine("Список товаров на складе: ").Font("Times New Roman").FontSize(14).Bold();
            paragraph2.AppendLine();
            foreach (StroredProduct product in config.list.AllstroredProducts)
            {
                paragraph2.AppendLine("Название: " + product.product.Name).Font("Times New Roman").FontSize(14);
                paragraph2.AppendLine("Полочка: " + product.polka).Font("Times New Roman").FontSize(12);
                paragraph2.AppendLine("Количество: " + product.count).Font("Times New Roman").FontSize(12);
                paragraph2.AppendLine("Поставщик: " + product.Postavshick.name).Font("Times New Roman").FontSize(12);
                paragraph2.AppendLine();
            }
            paragraph2.AppendLine("Список заказов: ").Font("Times New Roman").FontSize(14).Bold();
            paragraph2.AppendLine();
            foreach (Zakaz product in config.list.AllZakazs)
            {
                paragraph2.AppendLine("Название товара: " + product.stroredProduct.Name).Font("Times New Roman").FontSize(14);              
                paragraph2.AppendLine("Поставщик: " + product.postavshick.name).Font("Times New Roman").FontSize(14);
                paragraph2.AppendLine("Информация по данному поставщику: ").Font("Times New Roman").FontSize(14);
                paragraph2.AppendLine("Адрес: "+product.postavshick.adress);
                paragraph2.AppendLine("Телефон: " + product.postavshick.tel);
            }
            document.Save();
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Отчет" + ".docx");
        }
    }
}
