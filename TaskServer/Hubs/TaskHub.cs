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
        
        public async Task UpdateDbAsync()
        {
            List<Assignment> assignmentsSendDb = db.Assignments.ToList();
            await Clients.Caller.SendAsync("ReceiveDb", assignmentsSendDb);
        }

        public async Task AddAsync(Assignment assignment)
        {
            db.Assignments.Add(assignment);
            db.SaveChanges();
            List<Assignment> assignmentsAdd = db.Assignments.ToList();
            string messageAdd = "Задание добавлено!";
            await Clients.Caller.SendAsync("ReceiveAdd", assignmentsAdd, messageAdd);
        }

        public async Task StartedOnAsync(Assignment assignment)
        {
            
            if (assignment.CompletedOn != null)
            {
                List<Assignment> assignmentsStartedOn = db.Assignments.ToList();
                string messageStartedOn = "Задача уже выполнена!";
                await Clients.Caller.SendAsync("ReceiveStartedOn", assignmentsStartedOn, messageStartedOn);
            }
            else
            {
                if (assignment.StartedOn != null)
                {
                    List<Assignment> assignmentsStartedOn = db.Assignments.ToList();
                    string messageStartedOn = "Задача выполняется в настоящий момент!";
                    await Clients.Caller.SendAsync("ReceiveStartedOn", assignmentsStartedOn, messageStartedOn);
                }
                else
                {
                    assignment.StartedOn = DateTime.Now;
                    db.Assignments.Update(assignment);
                    db.SaveChanges();
                    List<Assignment> assignmentsStartedOn = db.Assignments.ToList();
                    string messageStartedOn = "Начато выполнение задачи!";
                    await Clients.Caller.SendAsync("ReceiveStartedOn", assignmentsStartedOn, messageStartedOn);
                }   
            }
           
        }

        public async Task CompletedOnAsync(Assignment assignment)
        {
            if(assignment.StartedOn != null)
            {
                if (assignment.CompletedOn != null)
                {
                    List<Assignment> assignmentsStartedOn = db.Assignments.ToList();
                    string messageStartedOn = "Задача уже выполнена!";
                    await Clients.Caller.SendAsync("ReceiveStartedOn", assignmentsStartedOn, messageStartedOn);
                }
                else
                {
                    assignment.CompletedOn = DateTime.Now;
                    db.Assignments.Update(assignment);
                    db.SaveChanges();
                    List<Assignment> assignmentsCompletedOn = db.Assignments.ToList();
                    string messageCompletedOn = "Задача завершена!";
                    await Clients.Caller.SendAsync("ReceiveCompletedOn", assignmentsCompletedOn, messageCompletedOn);
                }
            }
            else   {
                    
                    List<Assignment> assignmentsCompletedOn = db.Assignments.ToList();
                    string messageCompletedOn = "Задача еще на начата!";
                    await Clients.Caller.SendAsync("ReceiveCompletedOn", assignmentsCompletedOn, messageCompletedOn);
                   }
        }

        public async Task DeleteTaskDbAsync(int? id)
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
            await Clients.Caller.SendAsync("ReceiveDeleteTaskDb", assignmentsDeleteTaskDb, messageDeleteTaskD);
        }

    }
}
