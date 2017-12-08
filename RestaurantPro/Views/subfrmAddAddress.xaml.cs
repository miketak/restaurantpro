using BusinessLogic;
using DataObjects;
using DataObjects.ProgramDataObjects;
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
    /// Interaction logic for subfrmAddAddress.xaml
    /// </summary>
    public partial class subfrmAddAddress
    {
        /// <summary>
        /// Instance variable for each address
        /// </summary>
        Address _primary, _secondary1, _secondary2, _secondary3, _secondary4;

        /// <summary>
        /// Address instance variable from employee page
        /// </summary>
        static List<Address> _addressList;

        /// <summary>
        /// List of addressesOfEmployee types
        /// </summary>
        List<AddressType> _addressTypes;

        /// <summary>
        /// List of States
        /// </summary>
        List<State> _states;

        /// <summary>
        /// Indicates if in editAddress mode
        /// </summary>
        bool isEditMode;

        /// <summary>
        /// Countries
        /// </summary>
        List<Country> _countries;

        /// <summary>
        /// Holds old Address Type FeatureName for Combo box selection change
        /// </summary>
        string _oldAddressTypeName;

        /// <summary>
        /// Constructor call in edit mode.
        /// </summary>
        public subfrmAddAddress(List<Address> address)
        {
            _addressList = address;
            loadFormData();
            isEditMode = true;
            InitializeComponent();
            WindowSetup();
        }

        /// <summary>
        /// Constructor call in add mode
        /// </summary>
        public subfrmAddAddress()
        {
            loadFormData();
            isEditMode = false;
            InitializeComponent();
            WindowSetup();
        }

        private void splitAddresses()
        {
            Console.WriteLine("Why I'm I here");


            foreach (var add in _addressList)
            {
                if (add.AddressTypeId == 1)
                {
                    _primary = add;
                }
                else if (add.AddressTypeId == 2)
                {
                    _secondary1 = add;
                }
                else if (add.AddressTypeId == 3)
                {
                    _secondary2 = add;
                }
                else if (add.AddressTypeId == 4)
                {
                    _secondary3 = add;
                }
                else if (add.AddressTypeId == 5)
                {
                    _secondary4 = add;
                }
            }
        }

        /// <summary>
        /// Loads all form data for controls
        /// </summary>
        private void loadFormData()
        {
            var employeeManager = new EmployeeManager();
            var utilityManager = new UtilityManager();

            //split Addresses
            if ( _addressList != null)
                splitAddresses();

            // load Addresses types
            _addressTypes = employeeManager.RetrieveAddressTypeByID(-1, true);

            // load Countries
            _countries = utilityManager.RetrieveCountries();

            // load States
            _states = utilityManager.RetrieveStates();

        }

        /// <summary>
        /// Sets Up Window
        /// </summary>
        private void WindowSetup()
        {
            if (isEditMode)
            {
                this.Title = "Edit Address";

                fillComboBoxes();

                //Set Address Type to Index 0
                AddressType addressType = _addressTypes.Find(x => x.AddressTypeId == _addressList[0].AddressTypeId);
                cmbAddressType.SelectedItem = addressType.Name;
                _oldAddressTypeName = addressType.Name; //setting instance variable

                //Initial Setting
                txtAddressLine1.Text = _addressList[0].AddressLines[0];
                txtAddressLine2.Text = _addressList[0].AddressLines[1];
                txtAddressLine3.Text = _addressList[0].AddressLines[2];
                txtCity.Text = _addressList[0].City;
                txtZip.Text = _addressList[0].Zip;

                //Set Employee State to Index 0
                var employeesState = _states.Find(x => x.StateID == _addressList[0].StateID);
                cmbState.SelectedItem = employeesState.StateCode;

                //Set Employee Country to Index 0
                Country employeesCountry = _countries.Find(x => x.CountryID == _addressList[0].CountryID);
                cmbCountry.SelectedItem = employeesCountry.NiceName;

            }
            else
            {
                this.Title = "Add Address";
                _oldAddressTypeName = "Primary";
                fillComboBoxes();
            }

        }

        /// <summary>
        /// Fills Combo Boxes
        /// </summary>
        public void fillComboBoxes()
        {
            //fill country country box
            foreach (Country c in _countries)
            {
                cmbCountry.Items.Add(c.NiceName);
            }

            //load states combo box
            foreach (State st in _states)
            {
                cmbState.Items.Add(st.StateCode);
            }

            //load types into combo box
            foreach (var ad in _addressTypes)
            {
                cmbAddressType.Items.Add(ad.Name);
            }

        }

        private void cmbAddressType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_addressList != null)
            {

                updateTargetAddress();

                clearControls();

                //Get Selection and addresstypeID
                string selectedAddressText = (string)cmbAddressType.SelectedItem;
                var selectedAddress = _addressTypes.Find(x => x.Name == selectedAddressText);

                switch (selectedAddress.AddressTypeId)
                {
                    case 1:
                        setControls(_primary);
                        break;
                    case 2:
                        setControls(_secondary1);
                        break;
                    case 3:
                        setControls(_secondary2);
                        break;
                    case 4:
                        setControls(_secondary3);
                        break;
                    case 5:
                        setControls(_secondary4);
                        break;
                    default:
                        break;
                } // end of switch case

                //Reset _oldAddressTypeName
                _oldAddressTypeName = (string)cmbAddressType.SelectedItem;
            } //end of if statement
        }

        private void updateTargetAddress()
        {
            if (_oldAddressTypeName != null && addressEntryTextCount() != 0)
            {
                var oldAddressTypeID = _addressTypes.Find(x => x.Name == _oldAddressTypeName).AddressTypeId;
               

                switch (oldAddressTypeID)
                {
                    case 1:
                        _primary = new Address();
                        _primary.AddressTypeId = oldAddressTypeID;

                        if ( _addressList != null )
                        {
                            if (_addressList.Count > 0)
                                _primary.UserAddressId = _addressList[0].UserAddressId;
                        }                        

                        List<string> addresslines = new List<string>();
                        addresslines.Add(txtAddressLine1.Text);
                        addresslines.Add(txtAddressLine2.Text);
                        addresslines.Add(txtAddressLine3.Text);
                        _primary.AddressLines = addresslines;

                        _primary.City = txtCity.Text;
                        _primary.Zip = txtZip.Text;
                        if (cmbState.SelectedItem != null)
                            _primary.StateID = _states.Find(x => x.StateCode == (string)cmbState.SelectedItem).StateID;
                        if (cmbCountry.SelectedItem != null)
                            _primary.CountryID = _countries.Find(x => x.NiceName == (string)cmbCountry.SelectedItem).CountryID;
                        break;
                    case 2:
                        _secondary1 = new Address();
                        _secondary1.AddressTypeId = oldAddressTypeID;

                        if ( _addressList != null )
                        {
                            if (_addressList.Count > 1)
                                _secondary1.UserAddressId = _addressList[1].UserAddressId;
                        }
                        

                        addresslines = new List<string>();
                        addresslines.Add(txtAddressLine1.Text);
                        addresslines.Add(txtAddressLine2.Text);
                        addresslines.Add(txtAddressLine3.Text);
                        _secondary1.AddressLines = addresslines;

                        _secondary1.City = txtCity.Text;
                        _secondary1.Zip = txtZip.Text;
                        if (cmbState.SelectedItem != null)
                            _secondary1.StateID = _states.Find(x => x.StateCode == (string)cmbState.SelectedItem).StateID;
                        if (cmbCountry.SelectedItem != null)
                            _secondary1.CountryID = _countries.Find(x => x.NiceName == (string)cmbCountry.SelectedItem).CountryID;
                        break;
                    case 3:
                        _secondary2 = new Address();
                        _secondary2.AddressTypeId = oldAddressTypeID;

                        if ( _addressList != null )
                        {
                            if (_addressList.Count > 2)
                                _secondary2.UserAddressId = _addressList[2].UserAddressId;
                        }                        

                        addresslines = new List<string>();
                        addresslines.Add(txtAddressLine1.Text);
                        addresslines.Add(txtAddressLine2.Text);
                        addresslines.Add(txtAddressLine3.Text);
                        _secondary2.AddressLines = addresslines;

                        _secondary2.City = txtCity.Text;
                        _secondary2.Zip = txtZip.Text;
                        if (cmbState.SelectedItem != null)
                            _secondary2.StateID = _states.Find(x => x.StateCode == (string)cmbState.SelectedItem).StateID;
                        if (cmbCountry.SelectedItem != null)
                            _secondary2.CountryID = _countries.Find(x => x.NiceName == (string)cmbCountry.SelectedItem).CountryID;
                        break;
                    case 4:
                        _secondary3 = new Address();
                        _secondary3.AddressTypeId = oldAddressTypeID;
                        _secondary3.UserAddressId = _addressList[3].UserAddressId;

                        addresslines = new List<string>();
                        addresslines.Add(txtAddressLine1.Text);
                        addresslines.Add(txtAddressLine2.Text);
                        addresslines.Add(txtAddressLine3.Text);
                        _secondary3.AddressLines = addresslines;

                        _secondary3.City = txtCity.Text;
                        _secondary3.Zip = txtZip.Text;
                        if (cmbState.SelectedItem != null)
                            _secondary3.StateID = _states.Find(x => x.StateCode == (string)cmbState.SelectedItem).StateID;
                        if (cmbCountry.SelectedItem != null)
                            _secondary3.CountryID = _countries.Find(x => x.NiceName == (string)cmbCountry.SelectedItem).CountryID;
                        break;
                    case 5:
                        _secondary4 = new Address();
                        _secondary4.AddressTypeId = oldAddressTypeID;

                        if ( _addressList != null )
                        {
                            if (_addressList.Count > 4)
                                _secondary4.UserAddressId = _addressList[4].UserAddressId;
                        }                        

                        addresslines = new List<string>();
                        addresslines.Add(txtAddressLine1.Text);
                        addresslines.Add(txtAddressLine2.Text);
                        addresslines.Add(txtAddressLine3.Text);
                        _secondary4.AddressLines = addresslines;

                        _secondary4.City = txtCity.Text;
                        _secondary4.Zip = txtZip.Text;
                        if (cmbState.SelectedItem != null)
                            _secondary4.StateID = _states.Find(x => x.StateCode == (string)cmbState.SelectedItem).StateID;
                        if (cmbCountry.SelectedItem != null)
                            _secondary4.CountryID = _countries.Find(x => x.NiceName == (string)cmbCountry.SelectedItem).CountryID;
                        break;
                    default:
                        break;
                } // end of switch case

            }
        }

        /// <summary>
        /// Counts Entries in text boxes
        /// </summary>
        /// <returns></returns>
        private int addressEntryTextCount()
        {
            return ( txtAddressLine1.Text.Count() + txtAddressLine2.Text.Count() + txtAddressLine3.Text.Count() + txtCity.Text.Count() + txtZip.Text.Count() );
        }


        /// <summary>
        /// Clears all form controls
        /// </summary>
        private void clearControls()
        {
            txtAddressLine1.Clear();
            txtAddressLine2.Clear();
            txtAddressLine3.Clear();
            txtCity.Clear();
            cmbState.SelectedItem = null;
            txtZip.Clear();
            cmbCountry.SelectedItem = null;
        }

        /// <summary>
        /// Sets Form Controls
        /// </summary>
        /// <param name="selectedAddress"></param>
        private void setControls(Address selectedAddress)
        {

            if (selectedAddress != null)
            {
                //Set New Information
                txtAddressLine1.Text = selectedAddress.AddressLines[0];
                txtAddressLine2.Text = selectedAddress.AddressLines[1];
                txtAddressLine3.Text = selectedAddress.AddressLines[2];
                txtCity.Text = selectedAddress.City;
                txtZip.Text = selectedAddress.Zip;

                //Set Combo Boxes
                //State
                if ((int)selectedAddress.StateID != 0)
                {
                    State employeesState = _states.Find(x => x.StateID == selectedAddress.StateID);
                    cmbState.SelectedItem = employeesState.StateCode;
                }

                if ((int)selectedAddress.CountryID != 0)
                {
                    //Country
                    Country employeesCountry = _countries.Find(x => x.CountryID == selectedAddress.CountryID);
                    cmbCountry.SelectedItem = employeesCountry.NiceName;
                }

            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Updates Local Address List
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Perform last update
            updateTargetAddress();

            //Update _addressList to parent form
            if (_addressList != null)
                _addressList.Clear();
            else //Initialize Object
            {
                _addressList = new List<Address>();
            }

            if ( _primary != null )
            {
                
                _addressList.Add(_primary);
            }
            if (_secondary1 != null)
            {
                _addressList.Add(_secondary1);
            }
            if (_secondary2 != null)
            {
                _addressList.Add(_secondary2);
            }
            if (_secondary3 != null)
            {
                _addressList.Add(_secondary3);
            }
            if (_secondary4 != null)
            {
                _addressList.Add(_secondary4);
            }

            //Raise event to update parent form
            if ( _addressList != null)
            {
                this.UpdateParentForm(e);

                this.Close();
            }
            else //Notify User that nothing was loaded
            {
                MessageBox.Show("No Addresses Saved");
                this.Close();
            }

            
        }

        /// <summary>
        /// Event Handler to update parent form
        /// </summary>
        public static event EventHandler UpdateEvent;
        private void UpdateParentForm(EventArgs e)
        {
            if (_addressList != null)
            {
                UpdateEvent(this, e);
            }
        }

        /// <summary>
        /// Method Declaration for Parent form Call
        /// </summary>
        /// <returns></returns>
        public static List<Address> getUpdatedAddress()
        {
            return _addressList;
        }


    }
}
