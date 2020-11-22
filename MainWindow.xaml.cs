using AutoLotModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Preda_Marcela_Lab6.Validation;

namespace Preda_Marcela_Lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        CollectionViewSource customerViewSource;
        Binding txtFirstNameBinding = new Binding();
        Binding txtLastNameBinding = new Binding();

        CollectionViewSource inventoryViewSource;
        Binding txtColorBindig = new Binding();
        Binding txtMakeBinding = new Binding();

        CollectionViewSource customerOrdersViewSource;
        Binding txtCustId = new Binding();
        Binding txtCarId = new Binding();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            txtFirstNameBinding.Path = new PropertyPath("FirstName");
            txtLastNameBinding.Path = new PropertyPath("LastName");
            firstNameTextBox.SetBinding(TextBox.TextProperty, txtFirstNameBinding);
            lastNameTextBox.SetBinding(TextBox.TextProperty, txtLastNameBinding);

            txtColorBindig.Path = new PropertyPath("Color");
            txtMakeBinding.Path = new PropertyPath("Make");
            colorTextBox.SetBinding(TextBox.TextProperty, txtColorBindig);
            makeTextBox.SetBinding(TextBox.TextProperty, txtMakeBinding);

            txtCustId.Path = new PropertyPath("CustId");
            txtCarId.Path = new PropertyPath("CarId");
            custIdTextBox.SetBinding(TextBox.TextProperty, txtCustId);
            carIdTextBox.SetBinding(TextBox.TextProperty, txtCarId);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // customerViewSource.Source = [generic data source]
            customerViewSource =
           ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerViewSource.Source = ctx.Customers.Local;
            ctx.Customers.Load();
            System.Windows.Data.CollectionViewSource inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // inventoryViewSource.Source = [generic data source]
            inventoryViewSource.Source = ctx.Inventories.Local;
            ctx.Inventories.Load();

            customerOrdersViewSource =
            ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            //customerOrdersViewSource.Source = ctx.Orders.Local;
            ctx.Orders.Load();

            cmbCustomers.ItemsSource = ctx.Customers.Local;
            //cmbCustomers.DisplayMemberPath = "FirstName";
            cmbCustomers.SelectedValuePath = "CustId";
            cmbInventory.ItemsSource = ctx.Inventories.Local;
            //cmbInventory.DisplayMemberPath = "Make";
            cmbInventory.SelectedValuePath = "CarId";

        }
        private void BindDataGrid()
        {
            var queryOrder = from ord in ctx.Orders
                             join cust in ctx.Customers on ord.CustId equals
                             cust.CustId
                             join inv in ctx.Inventories on ord.CarId
                 equals inv.CarId
                             select new
                             {
                                 ord.OrderId,
                                 ord.CarId,
                                 ord.CustId,
                                 cust.FirstName,
                                 cust.LastName,
                                 inv.Make,
                                 inv.Color
                             };
            customerOrdersViewSource.Source = queryOrder.ToList();

        }
        private void SetValidationBinding()
        {
            Binding firstNameValidationBinding = new Binding();
            firstNameValidationBinding.Source = customerViewSource;
            firstNameValidationBinding.Path = new PropertyPath("FirstName");
            firstNameValidationBinding.NotifyOnValidationError = true;
            firstNameValidationBinding.Mode = BindingMode.TwoWay;
            firstNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string required
            firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            firstNameTextBox.SetBinding(TextBox.TextProperty,
           firstNameValidationBinding);
            Binding lastNameValidationBinding = new Binding();
            lastNameValidationBinding.Source = customerViewSource;
            lastNameValidationBinding.Path = new PropertyPath("LastName");
            lastNameValidationBinding.NotifyOnValidationError = true;
            lastNameValidationBinding.Mode = BindingMode.TwoWay;
            lastNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string min length validator
            lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValid());
            lastNameTextBox.SetBinding(TextBox.TextProperty,
           lastNameValidationBinding); //setare binding nou
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;

            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            customerDataGrid.IsEnabled = false;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            firstNameTextBox.IsEnabled = false;
            lastNameTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempFirstName = firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();

            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            customerDataGrid.IsEnabled = false;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            firstNameTextBox.IsEnabled = false;
            lastNameTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);

            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;
            
            SetValidationBinding();

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempFirstName = firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();

            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            customerDataGrid.IsEnabled = false;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            firstNameTextBox.IsEnabled = false;
            lastNameTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);

            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;

        }
       



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    customer = new Customer()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Customers.Add(customer);
                    customerViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;


            }
            if (action == ActionState.Edit)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;

                BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
                BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);

                customerViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                customerViewSource.View.MoveCurrentTo(customer);

            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;

                BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
                BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
                customerViewSource.View.Refresh();
                SetValidationBinding();
            }
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            customerDataGrid.IsEnabled = false;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            firstNameTextBox.IsEnabled = false;
            lastNameTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();

        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();

        }

        private void ordersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void btnNew1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew1.IsEnabled = false;
            btnEdit1.IsEnabled = false;
            btnDelete1.IsEnabled = false;

            btnSave1.IsEnabled = true;
            btnCancel1.IsEnabled = true;
            inventoryDataGrid.IsEnabled = false;
            btnPrev1.IsEnabled = false;
            btnNext1.IsEnabled = false;
            colorTextBox.IsEnabled = false;
            makeTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);

        }

        private void btnEdit1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempColor = colorTextBox.Text.ToString();
            string tempMake = makeTextBox.Text.ToString();

            btnNew1.IsEnabled = false;
            btnEdit1.IsEnabled = false;
            btnDelete1.IsEnabled = false;
            btnSave1.IsEnabled = true;
            btnCancel1.IsEnabled = true;
            inventoryDataGrid.IsEnabled = false;
            btnPrev1.IsEnabled = false;
            btnNext1.IsEnabled = false;
            colorTextBox.IsEnabled = false;
            makeTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);

            colorTextBox.Text = tempColor;
            makeTextBox.Text = tempMake;
        }

        private void btnDelete1_Click(object sender, RoutedEventArgs e)
        {

            action = ActionState.Delete;
            string tempColor = colorTextBox.Text.ToString();
            string tempMake = makeTextBox.Text.ToString();

            btnNew1.IsEnabled = false;
            btnEdit1.IsEnabled = false;
            btnDelete1.IsEnabled = false;
            btnSave1.IsEnabled = true;
            btnCancel1.IsEnabled = true;
            inventoryDataGrid.IsEnabled = false;
            btnPrev1.IsEnabled = false;
            btnNext1.IsEnabled = false;
            colorTextBox.IsEnabled = false;
            makeTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);

            colorTextBox.Text = tempColor;
            makeTextBox.Text = tempMake;
        }

        private void btnSave1_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    inventory = new Inventory()
                    {
                        Color = colorTextBox.Text.Trim(),
                        Make = makeTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Inventories.Add(inventory);
                    inventoryViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                btnNew1.IsEnabled = true;
                btnEdit1.IsEnabled = true;
                btnSave1.IsEnabled = false;
                btnCancel1.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPrev1.IsEnabled = true;
                btnNext1.IsEnabled = true;
                colorTextBox.IsEnabled = false;
                makeTextBox.IsEnabled = false;


            }
            if (action == ActionState.Edit)
            {
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    inventory.Color = colorTextBox.Text.Trim();
                    inventory.Make = makeTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                btnNew1.IsEnabled = true;
                btnEdit1.IsEnabled = true;
                btnDelete1.IsEnabled = true;
                btnSave1.IsEnabled = false;
                btnCancel1.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPrev1.IsEnabled = true;
                btnNext1.IsEnabled = true;
                colorTextBox.IsEnabled = false;
                makeTextBox.IsEnabled = false;

                BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
                BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);

                inventoryViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                inventoryViewSource.View.MoveCurrentTo(inventory);

            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    ctx.Inventories.Remove(inventory);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew1.IsEnabled = true;
                btnEdit1.IsEnabled = true;
                btnDelete1.IsEnabled = true;
                btnSave1.IsEnabled = false;
                btnCancel1.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPrev1.IsEnabled = true;
                btnNext1.IsEnabled = true;
                colorTextBox.IsEnabled = false;
                makeTextBox.IsEnabled = false;

                BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
                BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
                inventoryViewSource.View.Refresh();
            }
        }

        private void btnCancel1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew1.IsEnabled = true;
            btnEdit1.IsEnabled = true;
            btnEdit1.IsEnabled = true;
            btnSave1.IsEnabled = false;
            btnCancel1.IsEnabled = false;
            inventoryDataGrid.IsEnabled = false;
            btnPrev1.IsEnabled = false;
            btnNext1.IsEnabled = false;
            colorTextBox.IsEnabled = false;
            makeTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
        }

        private void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToNext();
        }

        private void btnPrev1_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToPrevious();
        }



        private void btnNew2_Click(object sender, RoutedEventArgs e)
        {  //nu sunt sigura ca e ok
            action = ActionState.New;
            btnNew2.IsEnabled = false;
            btnEdit2.IsEnabled = false;
            btnDelete2.IsEnabled = false;

            btnSave2.IsEnabled = true;
            btnCancel2.IsEnabled = true;
            ordersDataGrid.IsEnabled = false;
            btnPrev2.IsEnabled = false;
            btnNext2.IsEnabled = false;
            custIdTextBox.IsEnabled = false;
            carIdTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(carIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);
        }

        private void btnEdit2_Click(object sender, RoutedEventArgs e)
        {
            //nu sunt sigura ca e ok
            action = ActionState.Edit;
            string tempcustId = custIdTextBox.Text.ToString();
            string tempcarId = carIdTextBox.Text.ToString();

            btnNew2.IsEnabled = false;
            btnEdit2.IsEnabled = false;
            btnDelete2.IsEnabled = false;
            btnSave2.IsEnabled = true;
            btnCancel2.IsEnabled = true;
            ordersDataGrid.IsEnabled = false;
            btnPrev2.IsEnabled = false;
            btnNext2.IsEnabled = false;
            custIdTextBox.IsEnabled = false;
            carIdTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(carIdTextBox, TextBox.TextProperty);

            custIdTextBox.Text = tempcustId;
            carIdTextBox.Text = tempcarId;
        }

        private void btnDelete2_Click(object sender, RoutedEventArgs e)
        { //nu sunt sigura ca e ok
            action = ActionState.Delete;
            string tempcustId = custIdTextBox.Text.ToString();
            string tempcarId = carIdTextBox.Text.ToString();

            btnNew2.IsEnabled = false;
            btnEdit2.IsEnabled = false;
            btnDelete2.IsEnabled = false;
            btnSave2.IsEnabled = true;
            btnCancel2.IsEnabled = true;
            ordersDataGrid.IsEnabled = false;
            btnPrev2.IsEnabled = false;
            btnNext2.IsEnabled = false;
            custIdTextBox.IsEnabled = false;
            carIdTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(carIdTextBox, TextBox.TextProperty);

            custIdTextBox.Text = tempcustId;
            carIdTextBox.Text = tempcarId;
        }

        private void btnSave2_Click(object sender, RoutedEventArgs e)
        {
            Order order = null;
            if (action == ActionState.New)
            {
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Inventory inventory = (Inventory)cmbInventory.SelectedItem;
                    //instantiem Order entity
                    order = new Order()
                    {

                        CustId = customer.CustId,
                        CarId = inventory.CarId
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Orders.Add(order);
                    customerOrdersViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew2.IsEnabled = true;
                btnEdit2.IsEnabled = true;
                btnSave2.IsEnabled = false;
                btnCancel2.IsEnabled = false;
                ordersDataGrid.IsEnabled = true;
                btnPrev2.IsEnabled = true;
                btnNext2.IsEnabled = true;
                custIdTextBox.IsEnabled = false;
                carIdTextBox.IsEnabled = false;
            }

            if (action == ActionState.Edit)
            {
                dynamic selectedOrder = ordersDataGrid.SelectedItem;
                {
                    try
                    {
                        int curr_id = selectedOrder.OrderId;
                        var editedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                        if (editedOrder != null)
                        {
                            editedOrder.CustId = Int32.Parse(cmbCustomers.SelectedValue.ToString());
                            editedOrder.CarId = Convert.ToInt32(cmbInventory.SelectedValue.ToString());
                            //salvam modificarile
                            ctx.SaveChanges();
                        }
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    BindDataGrid();
                    // pozitionarea pe item-ul curent
                    customerViewSource.View.MoveCurrentTo(selectedOrder);
                }


                 else if (action == ActionState.Delete)
                {
                    try
                    {
                        dynamic selectedOrder = ordersDataGrid.SelectedItem;
                        int curr_id = selectedOrder.OrderId;
                        var deletedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                        if (deletedOrder != null)
                        {
                            ctx.Orders.Remove(deletedOrder);
                            ctx.SaveChanges();
                            MessageBox.Show("Order Deleted Successfully", "Message");
                            BindDataGrid();
                        }
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }

        }


                // if (action == ActionState.Edit)
                //{
                //  try
                //{
                //  order = (Order)ordersDataGrid.SelectedItem;
                //order.CustId = custIdTextBox.Text.Trim(); //nu stiu sa fac
                //rder.CarId = carIdTextBox.Text.Trim();     //nu stiu
                //salvam modificarile
                //ctx.SaveChanges();
                // }
                // catch (DataException ex)
                // {
                //  MessageBox.Show(ex.Message);
                // }

                // btnNew2.IsEnabled = true;
                // btnEdit2.IsEnabled = true;
                //  btnDelete2.IsEnabled = true;
                //  btnSave2.IsEnabled = false;
                // btnCancel2.IsEnabled = false;
                // ordersDataGrid.IsEnabled = true;
                // btnPrev2.IsEnabled = true;
                // btnNext2.IsEnabled = true;
                // custIdTextBox.IsEnabled = false;
                //  carIdTextBox.IsEnabled = false;

                // BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);
                //  BindingOperations.ClearBinding(carIdTextBox, TextBox.TextProperty);
                // customerOrdersViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                //  customerOrdersViewSource.View.MoveCurrentTo(order);

                // }
                // else if (action == ActionState.Delete)
                //  {
                // try
                //  {
                //  order = (Order)ordersDataGrid.SelectedItem;
                //nu stiu cum sa fac aici //  ctx.Inventories.Remove(order);
                //      ctx.SaveChanges();
                // }
                // catch (DataException ex)
                // {
                //   MessageBox.Show(ex.Message);
                // }
                // btnNew2.IsEnabled = true;
                // btnEdit2.IsEnabled = true;
                //  btnDelete2.IsEnabled = true;
                //  btnSave2.IsEnabled = false;
                //  btnCancel2.IsEnabled = false;
                //   ordersDataGrid.IsEnabled = true;
                //  btnPrev2.IsEnabled = true;
                //  btnNext2.IsEnabled = true;
                // custIdTextBox.IsEnabled = false;
                // carIdTextBox.IsEnabled = false;

                // BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);
                // BindingOperations.ClearBinding(carIdTextBox, TextBox.TextProperty);
                // customerOrdersViewSource.View.Refresh();
                // }
            
        private void btnCancel2_Click(object sender, RoutedEventArgs e)
        { // nu sunt siguraa
            action = ActionState.Nothing;
            btnNew2.IsEnabled = true;
            btnEdit2.IsEnabled = true;
            btnEdit2.IsEnabled = true;
            btnSave2.IsEnabled = false;
            btnCancel2.IsEnabled = false;
            ordersDataGrid.IsEnabled = false;
            btnPrev2.IsEnabled = false;
            btnNext2.IsEnabled = false;
            custIdTextBox.IsEnabled = false;
            carIdTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(carIdTextBox, TextBox.TextProperty);
            //
        }

        private void btnNext2_Click(object sender, RoutedEventArgs e)
        {
            customerOrdersViewSource.View.MoveCurrentToNext();
        }

        private void btnPrev2_Click(object sender, RoutedEventArgs e)
        {
            customerOrdersViewSource.View.MoveCurrentToPrevious();

        }

    }
}




