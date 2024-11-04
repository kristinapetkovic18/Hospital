﻿using HospitalProject.Controller;
using HospitalProject.Core;
using HospitalProject.Model;
using HospitalProject.View.DoctorView.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalProject.View.DoctorView.Model
{
    public class PreviousExaminationsViewModel : BaseViewModel
    {

        public ObservableCollection<Anamnesis> Anamneses { get; set; }

        private RelayCommand editAnamnesisCommand;

        private Anamnesis selectedItem;

        private AnamnesisController anamnesisController;

        public PreviousExaminationsViewModel(MedicalRecord medicalRecord)
        {
            var app = System.Windows.Application.Current as App;
            anamnesisController = app.AnamnesisController;
            Anamneses = new ObservableCollection<Anamnesis>(anamnesisController.GetAnamnesisByMedicalRecord(medicalRecord.Patient.Id));
        }

        public RelayCommand EditAnamnesisCommand
        {
            get 
            {
                return editAnamnesisCommand ?? (editAnamnesisCommand = new RelayCommand(param => EditAnamnesisCommandExecute(), param => CanEditAnamnesisCommandExecute()));
            }
        }

        private bool CanEditAnamnesisCommandExecute()
        {
            return true;
        }

        private void EditAnamnesisCommandExecute()
        {
            EditAnamnesisWindow view = new EditAnamnesisWindow();
            view.DataContext = new EditAnamnesisViewModel(SelectedItem, view);
            view.ShowDialog();
        }

        public Anamnesis SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
    }
}
