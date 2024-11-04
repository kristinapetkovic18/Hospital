﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using Controller;
using HospitalProject.Controller;
using HospitalProject.Core;
using HospitalProject.Model;
using HospitalProject.View.WardenForms.Views;
using Model;

namespace HospitalProject.View.WardenForms.ViewModels
{
    public class RoomRenovationViewModel : BaseViewModel
    {
        private RoomRenovationController _roomRenovationController;
        private AppointmentController _appointmentController;
        private RoomControoler _roomControoler;

        private DateTime searchStartDate;
        private DateTime searchEndDate;
        private int daysDuration;
        private Room room;

        private Visibility visableTermins;

        private ObservableCollection<RoomRenovation> _generatedRenovationAppointments;
        private ObservableCollection<RoomRenovation> _roomRenovationItems;
        private RoomRenovation selectedRenovation;
        

        public RelayCommand SubmitCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }


        public RoomRenovationViewModel(Room selectedRoom)
        {
            InitializeControllers();
            InitialiseData(selectedRoom);
            InitialiseCommands();
            
        }

        private void InitialiseData(Room selectedRoom)
        {
            Room = selectedRoom;
            SearchStartDate = DateTime.Today;
            SearchEndDate = DateTime.Today;
            VisableTermins = Visibility.Hidden;
        }

        private void InitialiseCommands()
        {
            SearchCommand = new RelayCommand( parm=> SearchCommandExecute(), param => CanExecuteSearch());
            SubmitCommand = new RelayCommand( parm=> ExecuteSubmitComand(), param => CanExecuteSubmit());  
        }

        public RoomRenovationViewModel()
        {
        }

        private void InitializeControllers()
        {
            var app = System.Windows.Application.Current as App;

            _roomRenovationController = app.RenovationController;
            _roomControoler = app.RoomController;
            _appointmentController = app.AppointmentController;
        }
        
        public ObservableCollection<RoomRenovation> GeneratedRenovationAppointments
        {
            get
            {
                return _generatedRenovationAppointments;
            }
            set
            {
                _generatedRenovationAppointments = value;
                OnPropertyChanged(nameof(GeneratedRenovationAppointments));
            }
        }
        
        public DateTime SearchStartDate
        {
            get
            {
                return searchStartDate;
            }
            set
            {
                searchStartDate = value;
                OnPropertyChanged(nameof(SearchStartDate));
            }
        }
        
        public Visibility VisableTermins
        {
            get
            {
                return visableTermins;
            }
            set
            {
                visableTermins = value;
                OnPropertyChanged(nameof(VisableTermins));
            }
        }

        public DateTime SearchEndDate
        {
            get
            {
                return searchEndDate;
            }
            set
            {
                searchEndDate = value;
                OnPropertyChanged(nameof(SearchEndDate));
            }
        }

        public int DaysDuration
        {
            get
            {
                return daysDuration;
            }
            set
            {
                daysDuration = value;
                OnPropertyChanged(nameof(DaysDuration));
            }
        }

        public Room Room
        {
            get
            {
                return room;
            }
            set
            {
                room = value;
                OnPropertyChanged(nameof(Room));
            }
        }
        
        public RoomRenovation SelectedRenovation
        {
            get
            {
                return selectedRenovation;
            }
            set
            {
                selectedRenovation = value;
                OnPropertyChanged(nameof(SelectedRenovation));
            }
        }



        private void SearchCommandExecute()
        {
            VisableTermins = Visibility.Visible;
            
            GeneratedRenovationAppointments = new ObservableCollection<RoomRenovation>(
                _roomRenovationController.GenerateAvailableRenovationAppointments(ConvertDateTimeToDateOnly(SearchStartDate), ConvertDateTimeToDateOnly(SearchEndDate), Room,
                    DaysDuration));
            
        }
        
        private DateOnly ConvertDateTimeToDateOnly(DateTime date )
        {
            return new DateOnly(date.Year, date.Month,
                date.Day);
        }


        private bool CanExecuteSearch()
        {
            
            return SearchStartDate != default && SearchEndDate != default && !SearchStartDate.Equals(SearchEndDate) &&
                   DaysDuration != 0 && searchStartDate.AddDays(DaysDuration) <= searchEndDate && SearchStartDate <= searchEndDate && searchStartDate >= DateTime.Today;
        }

        private bool CanExecuteSubmit()
        {
            return SelectedRenovation != null;
        }

        private void ExecuteSubmitComand()
        {
            _roomRenovationController.Create(SelectedRenovation);
            _generatedRenovationAppointments.Remove(SelectedRenovation);

            ChangeMainViewToWardenRoomControl();
        }

        private void ChangeMainViewToWardenRoomControl()
        {
            WardenRoomControl wrc = new WardenRoomControl();
            MainViewModel.Instance.MomentalView = wrc;
        }
    }
}

