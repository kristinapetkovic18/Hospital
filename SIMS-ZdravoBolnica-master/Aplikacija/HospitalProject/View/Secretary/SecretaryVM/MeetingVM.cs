using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using HospitalProject.Controller;
using HospitalProject.Core;
using HospitalProject.Model;
using Model;
using HospitalProject.ValidationRules.DoctorValidation;
using HospitalProject.View.Secretary.SecretaryV;
using Syncfusion.Windows.Controls;

namespace HospitalProject.View.Secretary.SecretaryVM;

public class MeetingVM : BaseViewModel
{
    private int _id;
    private Doctor _selectedItemForRemoving;
    private Doctor _selectedItemForAdding;
    private bool _isWardenAdded;
    private DateTime _date;
    private TimeOnly _time;
    private RelayCommand _createMeeting;
    private RelayCommand _removeSelectedDoctorFromListCommand;
    private RelayCommand _addSelectedDoctorFromListCommand;

    private RelayCommand _startDemo;
    private MeetingsController _meetingsController;
    private DoctorController _doctorController;
    public List<Doctor> Doctors { get; set; }
    public IList<Doctor> DoctorsForMeeting { get; set; }
    private List<TimeOnly> _timeList = new List<TimeOnly>();

    public MeetingVM(ObservableCollection<Doctor> doctorsForMeeting)
    {
        var app = System.Windows.Application.Current as App;
        _meetingsController = app.MeetingsController;
        _doctorController = app.DoctorController;
        Doctors = new List<Doctor>(_doctorController.GetAll().ToList());
        DoctorsForMeeting = doctorsForMeeting;
        FillTimeCB();
    }
    public RelayCommand StartDemo
    {
        get
        {
            return _startDemo ?? (_startDemo = new RelayCommand(param => StartDemoExecute(), param => CanStartDemoExecute()));
        }
    }
    private void StartDemoExecute()
    {
       MeetingDemoV view = new MeetingDemoV();
        view.DataContext = new MeetingDemoVM();
        SecretaryMainViewVM.Instance.CurrentView = view;
    }
    private bool CanStartDemoExecute()
    {
        return true;
    }

    public MeetingVM()
    {

        var app = System.Windows.Application.Current as App;
        _meetingsController = app.MeetingsController;
        _doctorController = app.DoctorController;
        Doctors = new List<Doctor>(_doctorController.GetAll().ToList());
        DoctorsForMeeting = new ObservableCollection<Doctor>();
        FillTimeCB();
    }

    public Doctor SelectedItemForRemoving
    {
        get { return _selectedItemForRemoving; }
        set
        {
            _selectedItemForRemoving = value;
            OnPropertyChanged(nameof(SelectedItemForRemoving));
        }
    }

    public Doctor SelectedItemForAdding
    {
        get { return _selectedItemForAdding; }
        set
        {
            _selectedItemForAdding = value;
            OnPropertyChanged(nameof(SelectedItemForAdding));
        }
    }

    public DateTime Date
    {
        get { return _date; }
        set
        {
            _date = value;
            OnPropertyChanged(nameof(Date));
        }
    }

    public TimeOnly Time
    {
        get { return _time; }
        set
        {
            _time = value;
            OnPropertyChanged(nameof(Time));
        }
    }

    public List<TimeOnly> TimeCB
    {
        get { return _timeList; }

    }

    public void FillTimeCB()
    {
        TimeOnly time = new TimeOnly(8,0);
        for (int i = 0; i < 12; i++)
        {
            
            _timeList.Add(time.AddHours(i));
            
        }

    }

    public DateTime CreateDateTime()
    {
        DateOnly DateOnly = new DateOnly(Date.Year, Date.Month, Date.Day);
        
        return new DateTime(DateOnly.Year, DateOnly.Month, DateOnly.Day,Time.Hour, Time.Minute, Time.Second);
    }

    public bool IsWardenAdded
    {
        get { return _isWardenAdded; }
        set
        {
            _isWardenAdded = value;
            OnPropertyChanged(nameof(IsWardenAdded));
        }
    }


    public RelayCommand CreateMeeting
    {
        get
        {
            return _createMeeting ?? (_createMeeting = new RelayCommand(param => ExecuteCreateMeetingCommand(),
                param => CanExecuteCreateMeeting()));
        }
    }

    public RelayCommand Remove
    {
        get
        {
            return _removeSelectedDoctorFromListCommand ?? (_removeSelectedDoctorFromListCommand = new RelayCommand(
                param => ExecuteRemoveDoctorCommand(),
                param => CanExecuteRemoveDoctor()));
        }
    }

    public RelayCommand Add
    {
        get
        {
            return _addSelectedDoctorFromListCommand ?? (_addSelectedDoctorFromListCommand = new RelayCommand(
                param => ExecuteAddDoctorCommand(),
                param => CanExecuteAddDoctor()));
        }
    }
    private bool CanExecuteCreateMeeting()
    {
        //TimeFormatValidation
        return DoctorsForMeeting.Count != 0;
    }

    private bool CanExecuteAddDoctor()
    {
        return (SelectedItemForAdding!=null && DoctorAlreadyAdded());
    }
    
    private bool DoctorAlreadyAdded()
    {
        foreach (Doctor doctor in DoctorsForMeeting)
        {
            if (doctor.Id == SelectedItemForAdding.Id)
            {
                return false;
            }
        }
        return true ;
    }
    private bool CanExecuteRemoveDoctor()
    {
        return SelectedItemForRemoving!=null;
    }
    
    private void ExecuteCreateMeetingCommand()
    {
        _meetingsController.Create(new Meetings( _id, 4 , DoctorsForMeeting.ToList(), CreateDateTime(), 60, IsWardenAdded ));
        MessageBox.Show("Meeting created!", "note", MessageBoxButton.OK);
        DoctorsForMeeting.Clear();
        IsWardenAdded = false;
        
    }
    
    private void ExecuteAddDoctorCommand()
    {
        DoctorsForMeeting.Add(SelectedItemForAdding);
    }
    private void ExecuteRemoveDoctorCommand()
    {
        DoctorsForMeeting.Remove(SelectedItemForRemoving);
    }
  
    }