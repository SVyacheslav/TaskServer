using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TaskServer.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskServer.Hubs
{
    public class TaskHub: Hub
    {
        TaskContext db;
        public TaskHub(TaskContext context)
        {
            db = context;
        }
        
        public Task SendDb()
        {
            List<Assignment> assignmentsSendDb = db.Assignments.ToList();
            return Clients.Caller.SendAsync("ReceiveDb", assignmentsSendDb);
        }

        public Task Add(Assignment assignment)
        {
            db.Assignments.Add(assignment);
            db.SaveChanges();
            List<Assignment> assignmentsAdd = db.Assignments.ToList();
            string messageAdd = "Задание добавлено!";
            return Clients.Caller.SendAsync("ReceiveAdd", assignmentsAdd, messageAdd);
        }

        public Task StartedOn(Assignment assignment)
        {
            
            if (assignment.CompletedOn != null)
            {
                List<Assignment> assignmentsStartedOn = db.Assignments.ToList();
                string messageStartedOn = "Задача уже выполнена!";
                return Clients.Caller.SendAsync("ReceiveStartedOn", assignmentsStartedOn, messageStartedOn);
            }
            else
            {
                if (assignment.StartedOn != null)
                {
                    List<Assignment> assignmentsStartedOn = db.Assignments.ToList();
                    string messageStartedOn = "Задача выполняется в настоящий момент!";
                    return Clients.Caller.SendAsync("ReceiveStartedOn", assignmentsStartedOn, messageStartedOn);
                }
                else
                {
                    assignment.StartedOn = DateTime.Now;
                    db.Assignments.Update(assignment);
                    db.SaveChanges();
                    List<Assignment> assignmentsStartedOn = db.Assignments.ToList();
                    string messageStartedOn = "Начато выполнение задачи!";
                    return Clients.Caller.SendAsync("ReceiveStartedOn", assignmentsStartedOn, messageStartedOn);
                }   
            }
           
        }

        public Task CompletedOn(Assignment assignment)
        {
            
            if (assignment.CompletedOn != null)
            {
                List<Assignment> assignmentsStartedOn = db.Assignments.ToList();
                string messageStartedOn = "Задача уже выполнена!";
                return Clients.Caller.SendAsync("ReceiveStartedOn", assignmentsStartedOn, messageStartedOn);
            }
            else
            {
                assignment.CompletedOn = DateTime.Now;                                                    
                db.Assignments.Update(assignment);
                db.SaveChanges();
                List<Assignment> assignmentsCompletedOn = db.Assignments.ToList();
                string messageCompletedOn = "Задача завершена!";
                return Clients.Caller.SendAsync("ReceiveCompletedOn", assignmentsCompletedOn, messageCompletedOn);
            }
   
        }

        public Task DeleteTaskDb(int? id)
        {
            if (id != null)
            {
                Assignment assignment = db.Assignments.FirstOrDefault(p => p.Id == id);
                if (assignment != null)
                {
                    db.Assignments.Remove(assignment);
                    db.SaveChanges();
                }
            }
            List<Assignment> assignmentsDeleteTaskDb = db.Assignments.ToList();
            string messageDeleteTaskD = "Задание удалено!";
            return Clients.Caller.SendAsync("ReceiveDeleteTaskDb", assignmentsDeleteTaskDb, messageDeleteTaskD);
        }

    }
}
