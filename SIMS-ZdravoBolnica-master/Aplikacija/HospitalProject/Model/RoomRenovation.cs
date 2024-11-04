﻿using System;
using Model;

namespace HospitalProject.Model;

public class RoomRenovation : ViewModelBase
{
    private DateOnly startDate;
    private DateOnly endDate;
    private Room room;
    private bool isDone;
    private int id;

    public Room Room
    {
        get { return room; }
        set
        {
            room = value;
            OnPropertyChanged(nameof(Room));
        }
    }
    
    public int Id
    {
        get { return id; }
        set
        {
            id = value;
            OnPropertyChanged(nameof(Id));
        }
    }
    
    public DateOnly StartDate 
    { 
        get
        {
            return startDate;
        }
        set
        {
            startDate = value;
            OnPropertyChanged(nameof(StartDate));
        }
    }
    
    public DateOnly EndDate 
    { 
        get
        {
            return endDate;
        }
        set
        {
            endDate = value;
            OnPropertyChanged(nameof(EndDate));
        }
    }
    public bool IsDone
    {
        get
        {
            return isDone;
        }
        set
        {
            isDone = value;
            OnPropertyChanged(nameof(IsDone));
        }
    }

    public RoomRenovation() {}

    public RoomRenovation(DateOnly startDate, DateOnly endDate, int roomId, bool isDone,int id)
    {
        this.startDate = startDate;
        this.endDate = endDate;
        Room room = new Room(roomId);
        this.room = room;
        this.isDone = isDone;
        this.id = id;
    }

    public RoomRenovation(DateOnly startDate, DateOnly endDate, Room room)
    {
        this.startDate = startDate;
        this.endDate = endDate;
        this.room = room;
    }
}