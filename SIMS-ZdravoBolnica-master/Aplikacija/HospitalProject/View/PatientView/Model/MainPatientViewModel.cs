﻿using Controller;
using HospitalProject.Controller;
using HospitalProject.Core;
using HospitalProject.View.PatientView.View;
using HospitalProject.View.Util;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HospitalProject.ValidationRules.PatientValidation;
using HospitalProject.Model;
using System.Threading;

namespace HospitalProject.View.PatientView.Model
{
    public class MainPatientViewModel : BaseViewModel

    {

        private Appointment selectedItem;

        private IList<Doctor> _doctors;
        private DateTime _date;
        private int _duration;
        private String _time;
        private Doctor _doctor;
        private Patient _patient;

        private RelayCommand addCommand;
        private RelayCommand deleteCommand;
        private RelayCommand newAppointmentCommand;
        private RelayCommand editAppointmentCommand;
        private RelayCommand infoCommand;

        private Window window;
        private Thread thread;
        private Thread thread1;

        public ObservableCollection<Appointment> AppointmentItems { get; set; }
        public ObservableCollection<int> DoctorIds { get; set; }

        AppointmentController _appointmentController;
        PatientController _patientController;
        DoctorController _doctorController;
        UserController _userController;
        NotificationController _notificationController;
        CustomNotificationController _customNotificationController;


        private List<ComboBoxData<Doctor>> doctorComboBox = new List<ComboBoxData<Doctor>>();

        public List<ComboBoxData<Doctor>> DoctorComboBox
        {

            get
            {
                return doctorComboBox;
            }
            set
            {
                doctorComboBox = value;
                OnPropertyChanged(nameof(DoctorComboBox));
            }
        }

        public Appointment SelectedItem
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

        public Doctor DoctorData
        {
            get
            {
                return _doctor;
            }
            set
            {
                _doctor = value;
                OnPropertyChanged(nameof(DoctorData));
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public int Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        public String Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public MainPatientViewModel(Window window)
        {

            InstantiateControllers();
            InstantiateData();
            FillComboData();
            this.window = window;
            ThreadStart ts = new ThreadStart(StartNotificationThread);
            ThreadStart ts1 = new ThreadStart(StartCustomNotificationThread);
            thread = new Thread(ts);
            thread1 = new Thread(ts1);
            thread.Start();
            thread1.Start();
        }

        private void InstantiateControllers()
        {
            var app = System.Windows.Application.Current as App;
            _appointmentController = app.AppointmentController;
            _patientController = app.PatientController;
            _doctorController = app.DoctorController;
            _userController = app.UserController;
            _notificationController = app.NotificationController;
            _customNotificationController = app.CustomNotificationController;
        }

        private void InstantiateData()
        {
            _patient = _patientController.GetLoggedPatient(_userController.GetLoggedUser().Username);
            AppointmentItems = new ObservableCollection<Appointment>(_appointmentController.GetAllUnfinishedAppointmentsForPatient(_patient.Id).ToList());
            _doctors = _doctorController.GetAll().ToList();
            
        }

        private void FillComboData()
        {

            foreach (Doctor d in _doctors)
            {
                doctorComboBox.Add(new ComboBoxData<Doctor> { Name = d.FirstName + " " + d.LastName, Value = d });
            }

        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new RelayCommand(param => DeleteCommandExecute(), param => CanDeleteCommandExecute()));
            }
        }

        private bool CanDeleteCommandExecute()
        {
            return SelectedItem != null;
        }

        private void DeleteCommandExecute()
        {
            if (MessageBox.Show("Are you sure you want to cancel an appointment?", "Cancel an appointment", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _appointmentController.Delete(SelectedItem.Id);
                AppointmentItems.Remove(SelectedItem);
                _userController.IncreaseCounter();
                if(_userController.GetLoggedUser().IsBlocked)
                {
                    _userController.Logout();
                    window.Close();
                    Application.Current.MainWindow.Show();
                }
            }

        }




         public RelayCommand NewAppointmentCommand
         {
             get
             {
                 return newAppointmentCommand ?? (newAppointmentCommand = new RelayCommand(param => NewAppointmentCommandExecute(), param => CanNewAppointmentCommandExecute()));
             }
         }

         private bool CanNewAppointmentCommandExecute()
         {
             return true;
         }

         private void NewAppointmentCommandExecute()
         {
             NewAppointmentPatientView view = new NewAppointmentPatientView();
             view.DataContext = new NewAppointmentPatientViewModel(AppointmentItems,view);
             view.ShowDialog();
         }


        public RelayCommand EditAppointmentCommand
        {
            get
            {
                return editAppointmentCommand ?? (editAppointmentCommand = new RelayCommand(param => EditAppointmentCommandExecute(),
                                                                                            param => CanEditAppointmentCommandExecute()));
            }
        }

        private bool CanEditAppointmentCommandExecute()
        {
            return SelectedItem != null &&
            EditAppointmentValidation.LessThank24HoursCheck(selectedItem.Date);
        }

        private void EditAppointmentCommandExecute()
        {
            EditAppointmentPatientView view = new EditAppointmentPatientView();
            view.DataContext = new EditAppointmentPatientViewModel(SelectedItem, AppointmentItems,view);
            view.ShowDialog();
            if (_userController.GetLoggedUser().IsBlocked)
            {
                _userController.Logout();
                window.Close();
                Application.Current.MainWindow.Show();
            }
        }

        public RelayCommand InfoCommand
        {
            get
            {
                return infoCommand ?? (infoCommand = new RelayCommand(param => InfoCommandExecute(),
                                                                                            param => CanInfoCommandExecute()));
            }
        }

        private bool CanInfoCommandExecute()
        {
            return true;
        }

        private void InfoCommandExecute()
        {
            RecipesAndNotificationsView view = new RecipesAndNotificationsView();
            view.DataContext = new RecipesAndNotificationsViewModel(window);
            view.ShowDialog();
        }


        private DateTime parseTime()
        {
            String[] hoursAndMinutes = _time.Split(':');
            int hours = int.Parse(hoursAndMinutes[0]);
            int minutes = int.Parse(hoursAndMinutes[1]);
            return new DateTime(_date.Year, _date.Month, _date.Day, hours, minutes, 0);
        }

        private bool CanCreate()
        {
            TimeSpan timeSpan = new TimeSpan(0, _duration, 0); ;
            DateTime newAppEndDate = _date + timeSpan;
            DateTime existingAppointmentEndDate;
            foreach (Appointment appointment in AppointmentItems)
            {
                existingAppointmentEndDate = appointment.Date + new TimeSpan(0, Duration, 0);
                if (_date.Year == appointment.Date.Year && _date.Month == appointment.Date.Month && _date.Day == appointment.Date.Day)
                {
                    if (_date <= appointment.Date && newAppEndDate >= existingAppointmentEndDate)
                    {
                        return false;
                    }
                    else if (_date >= appointment.Date && newAppEndDate <= existingAppointmentEndDate)
                    {
                        return false;
                    }
                    else if (_date < appointment.Date && newAppEndDate < existingAppointmentEndDate && newAppEndDate > appointment.Date)
                    {
                        return false;
                    }
                    else if (_date > appointment.Date && _date < existingAppointmentEndDate && newAppEndDate > existingAppointmentEndDate)
                    {
                        return false;
                    }


                }

            }

            return true;
        }


        public void StartNotificationThread()
        {
            while (true)
            {
                Notification notification = _notificationController.CheckForNotifications(_patient);
                if (notification != null)
                {
                    MessageBox.Show(notification.Prescription.Description, notification.Name);
                }
                Thread.Sleep(60*100);

            }
        
        
        }

        public void StartCustomNotificationThread()
        {
            while (true)
            {
                CustomNotification customNotification = _customNotificationController.CheckForCustomNotifications(_patient);
                if (customNotification != null)
                {

                    MessageBox.Show(customNotification.Text);

                }


                Thread.Sleep(60 * 1000);



            }


        }

    }

}


