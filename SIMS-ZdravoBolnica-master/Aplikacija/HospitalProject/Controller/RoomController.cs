﻿using System.Collections;
using HospitalProject.Service;
using Model;
using System.Collections.Generic;
using HospitalProject.View.WardenForms.ViewModels;

namespace HospitalProject.Controller
{
    public class RoomControoler
    {
        private RoomService _roomService;

        public RoomControoler(RoomService roomService)
        {
            _roomService = roomService;
        }
        
        public Room Create(Room room)
        {
            return  _roomService.Create(room);
        }

        public IEnumerable<Room> GetAll()
        {
            return _roomService.getAll();
        }

        public void Update(Room room)
        {
            _roomService.Update(room);
        }
        
        public void Delete(int id)
        {
            _roomService.Delete(id);
        }

        public IEnumerable<EquipmentRoomModel> GenerateEquipementRooms(int id)
        {
            return _roomService.GenerateEquipmentRooms(id);
        }
        public IEnumerable<EquipmentRoomModel> GenerateAllEquipementRooms(int id)
        {
            return _roomService.GenerateAllEquipmentRooms(id);
        }

        public Room Get(int id)
        {
            return _roomService.Get(id);
        }

        public void UpdateRoomsEquipment(int source, int destination, int equipmentId,
            int quantity)
        {
            _roomService.UpdateRoomsEquipment(source,destination,equipmentId,quantity);
        }

        public List<Room> GetByRoomType(RoomType roomType)
        {
            return _roomService.GetByRoomType(roomType);
        }
    }
}
