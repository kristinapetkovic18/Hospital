﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using HospitalProject.Controller;
using HospitalProject.Core;
using HospitalProject.Model;
using HospitalProject.View.WardenForms.ViewModels;
using Syncfusion.UI.Xaml.Diagram;
using System.Windows.Controls.Primitives;

namespace HospitalProject.View.WardenForms.Views
{
    /// <summary>
    /// Interaction logic for AddingMedicineView.xaml
    /// </summary>
    public partial class AddingMedicineView : UserControl
    {
        
        public ObservableCollection<Equipement> MedicineItems { get; set; }
        public ObservableCollection<AddingMedicineAlergiesViewModel> AllergiesList { get; set; }
        
        public ObservableCollection<EquipmentCheckBoxModel> ReplacmentList { get; set; }

        private EquipementController _equipementController;
        private AllergiesController _allergiesController;
        public ObservableCollection<Allergies> SelectedAllergies { get; set; }
        public ObservableCollection<Equipement> SelectedReplacements { get; set; }

        public RelayCommand AddMedicineCommand { get; set; }

        private string selectedName;
        private int selectedQuantity;
        
       

        public AddingMedicineView(ObservableCollection<Equipement> medicineItems)
        {
            InstantiateControllers();
            InstantiateData(medicineItems);
        }

        public AddingMedicineView()
        {
        }

        private void InstantiateControllers()
        {
            var app = System.Windows.Application.Current as App;
            _equipementController = app.EquipementController;
            _allergiesController = app.AllergiesController;
        } 
        
        
        private void InstantiateData(ObservableCollection<Equipement> medicineItems)
        {
            InitializeComponent();
            DataContext = this;
            InitializeCollections(medicineItems);
            LoadAlergies();
            LoadReplacements();
            InstantiateCommands();
        }

        private void InitializeCollections(ObservableCollection<Equipement> medicineItems)
        {
            AllergiesList = new ObservableCollection<AddingMedicineAlergiesViewModel>();
            ReplacmentList = new ObservableCollection<EquipmentCheckBoxModel>();
            SelectedAllergies = new ObservableCollection<Allergies>();
            SelectedReplacements = new ObservableCollection<Equipement>();
            MedicineItems = medicineItems;
        }

        private void InstantiateCommands()
        {
            AddMedicineCommand = new RelayCommand(param => ExecuteAddingMedicineComand(), param => CanExecuteAddingMedicineComand());

        }

        private void LoadAlergies()
        {
            foreach (var alergie in _allergiesController.GetAll())
            {
                AllergiesList.Add(new AddingMedicineAlergiesViewModel(alergie));
            }
        }

        private void LoadReplacements()
        {
            foreach (Equipement replacement in _equipementController.GetAll())
            {
                ReplacmentList.Add(new EquipmentCheckBoxModel(replacement));
            }
        }
        private void ExecuteAddingMedicineComand()
        {
            Equipement newMedicine = new Equipement(SelectedQuantity,SelectedName,EquipementType.MEDICINE,SelectedAllergies.ToList(),SelectedReplacements.ToList());
            MedicineItems.Add(newMedicine);
            _equipementController.Create(newMedicine);
            ChangeMainViewToMedicinetView();
        }

        private void ChangeMainViewToMedicinetView()
        {
            MedicineViewModel wardenEquipemntViewModel = new MedicineViewModel();
            MainViewModel.Instance.MomentalView = wardenEquipemntViewModel;
        }

        private bool CanExecuteAddingMedicineComand()
        {
            return SelectedName != null && SelectedQuantity > 0;
        }
        
        
        public void ddlSupstances_SelectionChanged(object sender, SelectionChangedEventArgs e)  
        {  
  
        }  
        
        public void ddlReplacement_SelectionChanged(object sender, SelectionChangedEventArgs e)  
        {  
  
        }  
        public void ddlSupstances_TextChanged(object sender, TextChangedEventArgs e)  
        {  
            ddlSupstances.ItemsSource = AllergiesList.Where(x => x.Name.StartsWith(ddlSupstances.Text.Trim()));
        }  
        
        public void ddlReplacement_TextChanged(object sender, TextChangedEventArgs e)  
        {  
            ddlReplacements.ItemsSource = ReplacmentList.Where(x => x.Name.StartsWith(ddlSupstances.Text.Trim()));
        }  
        private void AllCheckbocx_CheckedAndUnchecked(object sender, RoutedEventArgs e)  
        {  
            BindListBOX();  
        }  
        
        private void AllCheckbox_Replacements_CheckedAndUnchecked(object sender, RoutedEventArgs e)  
        {  
            BindListBOXReplacements();  
        }  
        
        private void BindListBOXReplacements()  
        {  
            SelectedReplacements.Clear();  
            foreach(EquipmentCheckBoxModel replacement in ReplacmentList)  
            {
                if (replacement.IsChecked)
                {
                    SelectedReplacements.Add(new Equipement(replacement));
                }
            }  
        }  
        
        
        private void BindListBOX()  
        {  
            SelectedAllergies.Clear();  
            foreach(var alergie in AllergiesList)  
            {
                if (alergie.IsChecked)
                {
                    SelectedAllergies.Add(new Allergies(alergie));
                }
            }  
        }  
        
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public string SelectedName
        {
            get { return selectedName; }
            set
            {
                if (value != selectedName)
                {
                    selectedName = value;
                    OnPropertyChanged(nameof(SelectedName));
                }
            }
        }
        
        public int SelectedQuantity
        {
            get { return selectedQuantity; }
            set
            {
                if (value != selectedQuantity)
                {
                    selectedQuantity = value;
                    OnPropertyChanged(nameof(SelectedQuantity));
                }
            }
        }

        private void ddlSupstances_MouseEnter(object sender, MouseEventArgs e)
        {
            popup_add_medicene_help.PlacementTarget = ddlSupstances;
            popup_add_medicene_help.Placement = PlacementMode.Relative;
            popup_add_medicene_help.HorizontalOffset = -205;
            popup_add_medicene_help.VerticalOffset = -42;
            popup_add_medicene_help.IsOpen = true;
            PopupAdd.PopupText.Text = "Select medicines allergies";
        }

        private void ddlSupstances_MouseLeave(object sender, MouseEventArgs e)
        {
            popup_add_medicene_help.Visibility = Visibility.Collapsed;
            popup_add_medicene_help.IsOpen = false;
        }

        private void SelectedSupstancessList_MouseEnter(object sender, MouseEventArgs e)
        {
            popup_add_medicene_help.PlacementTarget = SelectedSupstancessList;
            popup_add_medicene_help.Placement = PlacementMode.Relative;
            popup_add_medicene_help.HorizontalOffset = -205;
            popup_add_medicene_help.VerticalOffset = 0;
            popup_add_medicene_help.IsOpen = true;
            PopupAdd.PopupText.Text = "Selected medicines allergies";
        }

        private void SelectedSupstancessList_MouseLeave(object sender, MouseEventArgs e)
        {
            popup_add_medicene_help.Visibility = Visibility.Collapsed;
            popup_add_medicene_help.IsOpen = false;
        }

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            popup_add_medicene_help.PlacementTarget = InsertName;
            popup_add_medicene_help.Placement = PlacementMode.Relative;
            popup_add_medicene_help.HorizontalOffset = -205;
            popup_add_medicene_help.VerticalOffset = -45;
            popup_add_medicene_help.IsOpen = true;
            PopupAdd.PopupText.Text = "Insert medicines name";
        }

        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            popup_add_medicene_help.Visibility = Visibility.Collapsed;
            popup_add_medicene_help.IsOpen = false;
        }

        private void InsertQuantity_MouseEnter(object sender, MouseEventArgs e)
        {
            popup_add_medicene_help.PlacementTarget = InsertQuantity;
            popup_add_medicene_help.Placement = PlacementMode.Relative;
            popup_add_medicene_help.HorizontalOffset = -205;
            popup_add_medicene_help.VerticalOffset = -45;
            popup_add_medicene_help.IsOpen = true;
            PopupAdd.PopupText.Text = "Insert medicines quantity";
        }

        private void InsertQuantity_MouseLeave(object sender, MouseEventArgs e)
        {
            popup_add_medicene_help.Visibility = Visibility.Collapsed;
            popup_add_medicene_help.IsOpen = false;
        }

        private void ddlReplacements_MouseEnter(object sender, MouseEventArgs e)
        {
            popup_add_medicene_help.PlacementTarget = ddlReplacements;
            popup_add_medicene_help.Placement = PlacementMode.Relative;
            popup_add_medicene_help.HorizontalOffset = -205;
            popup_add_medicene_help.VerticalOffset = -42;
            popup_add_medicene_help.IsOpen = true;
            PopupAdd.PopupText.Text = "Select medicines replacements";
        }

        private void ddlReplacements_MouseLeave(object sender, MouseEventArgs e)
        {
            popup_add_medicene_help.Visibility = Visibility.Collapsed;
            popup_add_medicene_help.IsOpen = false;
        }

        private void SelectedReplacementList_MouseEnter(object sender, MouseEventArgs e)
        {
            popup_add_medicene_help.PlacementTarget = SelectedReplacementList;
            popup_add_medicene_help.Placement = PlacementMode.Relative;
            popup_add_medicene_help.HorizontalOffset = -205;
            popup_add_medicene_help.VerticalOffset = 0;
            popup_add_medicene_help.IsOpen = true;
            PopupAdd.PopupText.Text = "Selected medicines replacements";
        }

        private void SelectedReplacementList_MouseLeave(object sender, MouseEventArgs e)
        {
            popup_add_medicene_help.Visibility = Visibility.Collapsed;
            popup_add_medicene_help.IsOpen = false;
        }
    }
    }

