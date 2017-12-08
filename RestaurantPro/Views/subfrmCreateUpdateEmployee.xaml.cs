using BusinessLogic;
using DataObjects;
using DataObjects.ProgramDataObjects;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace RestaurantPro
{
    /// <summary>
    /// Interaction logic for subfrmCreateUpdateEmployee.xaml
    /// </summary>
    public partial class subfrmCreateUpdateEmployee
    {
        /// <summary>
        /// Current user logged into the system
        /// </summary>
        User _user = new User();

        /// <summary>
        /// Indicates whether form is in editAddress mode
        /// </summary>
        bool _isEditMode;

        /// <summary>
        /// Employee from database
        /// </summary>
        Employee _employee = new Employee();

        /// <summary>
        /// Employee username from calling class.
        /// </summary>
        String _employeeUsername;

        /// <summary>
        /// Address Type list
        /// </summary>
        List<AddressType> _addressTypes; 

        /// <summary>
        /// Job positions field for form
        /// </summary>
        List<UserRoles> _jobPositions;

        /// <summary>
        /// List of countries
        /// </summary>
        List<Country> _countries;

        /// <summary>
        /// Department lists for departments combobox
        /// </summary>
        List<Department> _departments;

        /// <summary>
        /// Clearance Levels list for Clearance Level Combo Box
        /// </summary>
        List<ClearanceLevel> _clearanceLevels;

        /// <summary>
        /// Close event trigger
        /// </summary>
        bool actualClose = false;

        /// <summary>
        /// Constructor Edit Mode
        /// </summary>
        public subfrmCreateUpdateEmployee(User user, String employeeUsername)
        {
            _user = user;
            _employeeUsername = employeeUsername;
            _isEditMode = true;
            subfrmAddAddress.UpdateEvent += new EventHandler(Update_EmployeeAddress_Event);
            InitializeComponent();
            SetupWindow();
        }

        /// <summary>
        /// Constructor for Add Mode
        /// </summary>
        /// <param name="user"></param>
        public subfrmCreateUpdateEmployee(User user)
        {
            _user = user;
            _isEditMode = false;
            subfrmAddAddress.UpdateEvent += new EventHandler(Update_EmployeeAddress_Event);
            InitializeComponent();
            SetupWindow();

        }

        /// <summary>
        /// Update Employee Address Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void Update_EmployeeAddress_Event(object sender, EventArgs e)
        {
            //Update address
            if (_employee != null)
            {
                _employee.Address = subfrmAddAddress.getUpdatedAddress();
            }
            else //Instantiate Employee
            {
                _employee = new Employee();
                _employee.Address = subfrmAddAddress.getUpdatedAddress();
            }

            //MessageBox.Show("Hey I'm working");

            //Set Address Boxes
            //Refresh combo boxes - especially address drop down
            List<Address> addressesOfEmployee = _employee.Address;
            cmbAddressTypes.IsEnabled = true;
            cmbAddressTypes.Items.Clear();
            if (addressesOfEmployee.Count != 0)
            {
                foreach (var addType in addressesOfEmployee)
                {
                    AddressType addressType = _addressTypes.Find(x => x.AddressTypeId == addType.AddressTypeId);
                    cmbAddressTypes.Items.Add(addressType.Name);
                }
                cmbAddressTypes.SelectedItem = _addressTypes.Find(x => x.AddressTypeId == addressesOfEmployee[0].AddressTypeId).Name;
                // fill Address TextBox
                txtAddress.Clear();
                foreach (var adtext in addressesOfEmployee[0].AddressLines)
                {
                    if (!adtext.Equals(null))
                    {
                        txtAddress.Text += adtext + "\n";
                    }
                }
            }

            //Indicate Success
            var mySettings = new MetroDialogSettings()
           {
               AffirmativeButtonText = "Ok",
               ColorScheme = MetroDialogColorScheme.Theme
           };

            await this.ShowMessageAsync("Success", "Addresses Saved Successfully",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

        }

        /// <summary>
        /// Subform to editAddress and add new addresses to parent form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAddress(object sender, RoutedEventArgs e)
        {
            var subfrmAddAddress = new subfrmAddAddress();
            subfrmAddAddress.ShowDialog();
        }

        /// <summary>
        /// Subform to editAddress and add new new emails to parent form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPersonEmails(object sender, RoutedEventArgs e)
        {
            var subfrmAddEmail = new subfrmAddEmail();
            subfrmAddEmail.ShowDialog();
        }

        /// <summary>
        /// Subform to editAddress and add new personal telphone numbers to parent form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPersonalTelephone(object sender, RoutedEventArgs e)
        {
            var subfrmAddPersonalTelephone = new subfrmAddPersonalTelephone();
            subfrmAddPersonalTelephone.ShowDialog();
        }

        /// <summary>
        /// Sets up window elements from User information
        /// </summary>
        private void SetupWindow()
        {
            if (_isEditMode)
            {
                //Set up user environment
                txtNameTag.Text = _user.FirstName + " " + _user.LastName;

                //Sets _employee private field with employee data
                getEmployeeDetails(_employeeUsername);
                this.Title = "Edit Employee: " + _employee.FirstName + " " + _employee.LastName;

                //Set all combo boxes
                fillComboBoxes();

                //disable passwordbox
                txtPassword.IsEnabled = false;

                //Set all textboxes for Employee information
                txtUsername.Text = _employee.Username;
                txtFirstName.Text = _employee.FirstName;
                txtLastName.Text = _employee.LastName;
                txtOtherNames.Text = _employee.OtherNames;
                cmbDepartment.SelectedItem = _employee.Department;
                txtPersonalTelephone.Text = _employee.PersonalPhoneNumber;
                txtPersonalEmail.Text = _employee.PersonalEmail;

                cmbNationality.SelectedItem = _employee.Nationality;

                chkMaritalStatus.IsChecked = _employee.MaritalStatus ? true : false;

                if (_employee.Gender != null)
                    cmbGender.SelectedItem = _employee.Gender == true ? "Male" : "Female";

                dateDOB.SelectedDate = _employee.DateOfBirth;
                if (dateDOB.SelectedDate == DateTime.MinValue)
                {
                    dateDOB.SelectedDate = null;
                }

                txtCompanyTelephone.Text = _employee.PhoneNumber;
                txtCompanyEmail.Text = _employee.Email;
                cmbClearanceLevel.SelectedItem = _employee.ClearanceLevel;
                chkisActive.IsChecked = _employee.isEmployed;
                txtAdditionalInfo.Text = _employee.AdditonalInfo;

                //Get selected role
                UserRoles loadedEmployeeRole = _jobPositions.Find(x => x.UserRolesId == _employee.UserRolesId);
                cmbJobPosition.SelectedItem = loadedEmployeeRole.Name;

            }
            else
            {
                //Setup user environment
                txtNameTag.Text = _user.FirstName + " " + _user.LastName;

                //disable passwordbox
                txtPassword.IsEnabled = false;

                //fillComboBoxes
                fillComboBoxes();

            }

        }


        /// <summary>
        /// Fills all combo boxes
        /// </summary>
        private void fillComboBoxes()
        {
            var employeeManager = new EmployeeManager();
            var utilityManager = new UtilityManager();

            //Execute for Edit Mode - False or True------------------------------------------
            //fill gender types
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");

            //fill departments combobox
            _departments = employeeManager.RetrieveDepartmentsByVisibility(true);
            foreach (Department d in _departments)
            {
                cmbDepartment.Items.Add(d.Name);
            }

            //fill nationality box
            _countries = utilityManager.RetrieveCountries();
            foreach (Country c in _countries)
            {
                cmbNationality.Items.Add(c.NiceName);
            }

            //load addressesOfEmployee types into memory
            _addressTypes = employeeManager.RetrieveAddressTypeByID(-1, true);

            //----------------------------------------------------------------------------------

            if (_isEditMode) //editAddress mode
            {
                //fill jobs position box based on selected department               
                fillJobPositions(_employee.DepartmentId);

                //fill clearance levels
                _clearanceLevels = employeeManager.RetrieveClearanceByDeptID(_employee.DepartmentId, true);
                foreach (ClearanceLevel cl in _clearanceLevels)
                {
                    cmbClearanceLevel.Items.Add(cl.Name);
                }

                // fill Address types---------------------------------------------------------------------------------
                List<Address> addressesOfEmployee = _employee.Address;
                if (addressesOfEmployee.Count != 0)
                {
                    foreach (var addType in addressesOfEmployee)
                    {
                        AddressType addressType = _addressTypes.Find(x => x.AddressTypeId == addType.AddressTypeId);
                        cmbAddressTypes.Items.Add(addressType.Name);
                    }
                    cmbAddressTypes.SelectedItem = _addressTypes.Find(x => x.AddressTypeId == addressesOfEmployee[0].AddressTypeId).Name;
                    // fill Address TextBox
                    txtAddress.Clear();
                    foreach (var adtext in addressesOfEmployee[0].AddressLines)
                    {
                        if (!adtext.Equals(null))
                        {
                            txtAddress.Text += adtext + "\n";
                        }
                    }
                }
                //------------------------------------------------------------------------------------------------------



            }
            else //add mode
            {
                //fill clearance levels
                _clearanceLevels = employeeManager.RetrieveClearanceByDeptID("", true);
                foreach (ClearanceLevel cl in _clearanceLevels)
                {
                    cmbClearanceLevel.Items.Add(cl.Name);
                }

                //do not fill job positions box. indicate to user to select department
                cmbJobPosition.Items.Add("Select Department");

                // fill Address types
                string noAddressPrompt = "No Addresses Yet";
                cmbAddressTypes.Items.Add(noAddressPrompt);
                //cmbAddressTypes.SelectedItem = noAddressPrompt;
                cmbAddressTypes.IsEnabled = false;
            }


        }

        /// <summary>
        /// Changes Address Text Box Based on selected option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAddressTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("CMB ADDRESS Selection Change");

            if (_employee.Address != null)
            {
                //clear Address Text Box
                txtAddress.Clear();
                Address newAddressToDisplay = new Address();

                //Get Selection and addresstypeID
                if ((string)cmbAddressTypes.SelectedItem != null)
                {
                    string selectedAddressText = (string)cmbAddressTypes.SelectedItem;
                    var selectedAddress = _addressTypes.Find(x => x.Name == selectedAddressText);

                    //Search in employee object for addressesOfEmployee
                    var addressFromEmployee = _employee.Address;
                    newAddressToDisplay = addressFromEmployee.Find(x => x.AddressTypeId == selectedAddress.AddressTypeId);
                }

                if (newAddressToDisplay != null)
                {
                    //Set Address Box
                    if (newAddressToDisplay.AddressLines != null)
                    {
                        foreach (var adtext in newAddressToDisplay.AddressLines)
                        {
                            if (!adtext.Equals(null))
                            {
                                txtAddress.Text += adtext + "\n";
                            }
                        }
                    }
                }
                else
                {
                    txtAddress.Text = "No Address Saved";
                }


            }
        }


        /// <summary>
        /// Fill departments based on Department Id.
        /// </summary>
        /// <param name="departmentId"></param>
        private void fillJobPositions(string departmentId)
        {
            //fill job positions by department
            var employeeManager = new EmployeeManager();

            //clear job positions and combo box
            _jobPositions = null;
            cmbJobPosition.Items.Clear();

            //fill job positions
            _jobPositions = employeeManager.RetrieveUserRolesByDeptID(departmentId, false); //true means select all departments

            foreach (UserRoles roles in _jobPositions)
            {
                cmbJobPosition.Items.Add(roles.Name);
            }

        }

        /// <summary>
        /// Sets _employee id private field with employee basie date
        /// </summary>
        /// <param name="employeeUsername">Employee Username used for data retrieval</param>
        private void getEmployeeDetails(string employeeUsername)
        {
            var ld = new EmployeeManager();
            //Get full employee details
            try
            {
                _employee = ld.RetrieveEmployeeByUsername(employeeUsername);

            }
            catch (Exception e)
            {

                MessageBox.Show("Message" + e.Message);
            }


        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //clear _departments object
            _departments.Clear();

            //reload departments
            var employeeManager = new EmployeeManager();
            _departments = employeeManager.RetrieveDepartmentsByVisibility(true);


            //Get selected department id for _departments enumerable
            if (cmbDepartment.SelectedItem != null)
            {
                Department loadedDepartments = _departments.Find(x => x.Name == (string)cmbDepartment.SelectedItem);
                fillJobPositions(loadedDepartments.DepartmentId);
            }


        }

        private async void btnCancel(object sender, RoutedEventArgs e)
        {
            actualClose = true;
            var mySettings = new MetroDialogSettings()
           {
               AffirmativeButtonText = "Yes",
               NegativeButtonText = "Cancel",
               ColorScheme = MetroDialogColorScheme.Theme
           };

            MessageDialogResult result = await this.ShowMessageAsync("Warning", "Closing might result in loss of Data. Do you still want to cancel?",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
                this.Close();
            }

        }

        private void msdEditAddress(object sender, MouseButtonEventArgs e)
        {
            //Open form in edit mode
            if (txtAddress.Text.Count() == 0)
            {
                MessageBox.Show("Kindly add an address");
                txtStatusMessage.Text += "Kindly add an address";
            }
            else
            {
                var editAddress = new subfrmAddAddress(_employee.Address);
                editAddress.ShowDialog();
            }

        }

        private void txtAddress_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            if (txtStatusMessage.Text.Length < 17)
                txtStatusMessage.Text += "Double Click to Edit Address";

        }

        private void txtAddress_ToolTipClosing(object sender, ToolTipEventArgs e)
        {
            txtStatusMessage.Text = "Status Message: ";
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!actualClose)
            {
                e.Cancel = true;
            }

            var mySettings = new MetroDialogSettings()
           {
               AffirmativeButtonText = "Yes",
               NegativeButtonText = "Cancel",
               ColorScheme = MetroDialogColorScheme.Theme
           };

            MessageDialogResult result = await this.ShowMessageAsync("Warning", "Closing might result in loss of Data. Do you still want to cancel?",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
                actualClose = true;
                this.Close();
            }
            else
            {
                actualClose = false;
            }

        }

        private async void btnSave(object sender, RoutedEventArgs e)
        {
            //Validate User Input
            if (1 == ValidateInputs())
                return;

            //_employee = new Employee();

            _employee.FirstName = txtFirstName.Text;

            _employee.LastName = txtLastName.Text;

            _employee.OtherNames = txtOtherNames.Text;

            _employee.PersonalPhoneNumber = txtPersonalTelephone.Text;

            _employee.PersonalEmail = txtPersonalEmail.Text;

            // _employee.Address :: Already Set in Add/Edit Address SubForm

            if (cmbNationality.SelectedItem != null)
                _employee.CountryId = _countries.Find(x => x.NiceName == (string)cmbNationality.SelectedItem).CountryID;

            _employee.MaritalStatus = (bool)chkMaritalStatus.IsChecked;
            _employee.Gender = (string)cmbGender.SelectedItem == "Male" ? true : false;


            if (dateDOB.SelectedDate != null)
            {
                _employee.DateOfBirth = (DateTime)dateDOB.SelectedDate;
            }
            else
            {
                _employee.DateOfBirth = null;
            }

            
            _employee.Username = txtUsername.Text; //Create check for username existence

            _employee.PhoneNumber = txtCompanyTelephone.Text;
              
            _employee.Email = txtCompanyEmail.Text;


            if (cmbJobPosition.SelectedItem != null)
                _employee.UserRolesId = _jobPositions.Find(x => x.Name == (string)cmbJobPosition.SelectedItem).UserRolesId;

            if (cmbClearanceLevel.SelectedItem != null)
                _employee.ClearanceLevelId = _clearanceLevels.Find(x => x.Name == (string)cmbClearanceLevel.SelectedItem).ClearanceLevelId;

            _employee.isEmployed = (bool)chkisActive.IsChecked;

            _employee.AdditonalInfo = txtAdditionalInfo.Text;

            // Write Employee Data to Database
            string userMessage = null;
            string title = null;
            var employeeManager = new EmployeeManager();
            try
            {
                if (_employee.UserId == 0) //Create New Employee
                {
                    //MessageBox.Show("Creating");
                    if (employeeManager.CreateEmployee(_employee)){

                        title = "Success!";
                        userMessage = "Employee Created Successfully!";
                        ClearControls();
                    }                       
                    else
                    {
                        title = "Failure!";
                        userMessage = "There was an error saving your data!";
                        return;
                    }
                        
                }
                else //Update Employee
                {
                    //MessageBox.Show("Updating");
                    if (employeeManager.UpdateEmployeeByID(_employee))
                    {
                        title = "Success!";
                        userMessage = "Employee Updated Successfully!";
                    }
                    else
                    {
                        title = "Failure!";
                        userMessage = "There was an error in your update.";
                    }
                                       
                }
                
            }
            catch (Exception ex)
            {
                //throw;
                MessageBox.Show("Error: " + ex.Message);
            }

            //Success Message
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Ok",
                ColorScheme = MetroDialogColorScheme.Theme
            };

            await this.ShowMessageAsync(title,userMessage,
                MessageDialogStyle.Affirmative, mySettings);

        }
        //else //Update Employee
        //{
        //    MessageBox.Show("Eureka");
        //    var employeeManager = new EmployeeManager();
        //    employeeManager.UpdateEmployeeByID(_employee);
        //}
        //}

        /// <summary>
        /// Resets all controls and instance variables after successful creation
        /// </summary>
        private void ClearControls()
        {
            TraverseVisualTree(this);
            _employee = null;
        }

        /// <summary>
        ///  Clears All Controls in Form
        /// </summary>
        /// <param name="addEditEmployee"></param>
        static public void TraverseVisualTree(Visual addEditEmployee)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(addEditEmployee);
            for (int i = 0; i < childrenCount; i++)
            {
                var visualChild = (Visual)VisualTreeHelper.GetChild(addEditEmployee, i);
                if (visualChild is TextBox)
                {
                    TextBox tb = (TextBox)visualChild;
                    tb.Clear();
                }
                if (visualChild is ComboBox)
                {
                    ComboBox cb = (ComboBox)visualChild;
                    cb.SelectedItem = null;
                }
                TraverseVisualTree(visualChild);
            }
        }

        /// <summary>
        /// Validates input
        /// </summary>
        /// <returns></returns>
        private int ValidateInputs()
        {
            int signal = 0;

            if (txtFirstName.Text == "")
            {
                txtFirstName.BorderBrush = System.Windows.Media.Brushes.Red;
                signal = 1;

            }

            if (txtLastName.Text == "")
            {
                txtLastName.BorderBrush = System.Windows.Media.Brushes.Red;
                signal = 1;
            }

            if (txtPersonalTelephone.Text == "")
            {
                txtPersonalTelephone.BorderBrush = System.Windows.Media.Brushes.Red;
                signal = 1;
            }

            if (cmbJobPosition.SelectedItem == null)
            {
                cmbJobPosition.BorderBrush = System.Windows.Media.Brushes.Red;
                signal = 1;
            }

            if (cmbClearanceLevel.SelectedItem == null)
            {
                cmbClearanceLevel.BorderBrush = System.Windows.Media.Brushes.Red;
                signal = 1;
            }

            if (txtUsername.Text == "")
            {
                txtUsername.BorderBrush = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Kindly Generate Username");
                txtUsername.Focus();
                signal = 1;
            }

            return signal;
        }


    }
}
