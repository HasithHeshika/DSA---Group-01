using System;
using System.Collections.Generic;
using System.Diagnostics;

abstract class HealthcareResource
{
    public string? Name { get; set; }
    public string? ID { get; set; }
    public abstract void ManageResource();  // Abstract method for managing resource
}

class Doctor : HealthcareResource
{
    public string Specialty { get; set; }

    public Doctor(string name, string id, string specialty)
    {
        Name = name;
        ID = id;
        Specialty = specialty;
    }

    public override void ManageResource()
    {
        Console.WriteLine($"Managing appointment for doctor {Name}, Specialty: {Specialty}");
    }
}

class Room : HealthcareResource
{
    public string RoomType { get; set; }

    public Room(string name, string id, string roomType)
    {
        Name = name;
        ID = id;
        RoomType = roomType;
    }

    public override void ManageResource()
    {
        Console.WriteLine($"Managing room {Name}, Type: {RoomType}");
    }
}

class Appointment
{
    public string PatientName { get; set; }
    public DateTime AppointmentTime { get; set; }
    public int Priority { get; set; }  // Priority 1: Urgent, 2: Normal
    public Room Room { get; set; }

    public Appointment(string patientName, DateTime appointmentTime, int priority, Room room)
    {
        PatientName = patientName;
        AppointmentTime = appointmentTime;
        Priority = priority;
        Room = room;
    }
}

class PriorityQueue
{
    private List<Appointment> appointments = new List<Appointment>();

    public void Enqueue(Appointment appointment)
    {
        appointments.Add(appointment);
        appointments.Sort((x, y) => x.Priority.CompareTo(y.Priority));  // Simple sorting based on priority
    }

    public Appointment Dequeue()
    {
        if (appointments.Count == 0)
            throw new InvalidOperationException("No appointments in the queue.");
        var appointment = appointments[0];
        appointments.RemoveAt(0);
        return appointment;
    }
}

class SortingAlgorithms
{
    // Bubble Sort
    public static void BubbleSort(List<Appointment> appointments)
    {
        for (int i = 0; i < appointments.Count; i++)
        {
            for (int j = 0; j < appointments.Count - 1 - i; j++)
            {
                if (appointments[j].AppointmentTime > appointments[j + 1].AppointmentTime)
                {
                    var temp = appointments[j];
                    appointments[j] = appointments[j + 1];
                    appointments[j + 1] = temp;
                }
            }
        }
    }

    // Merge Sort
    public static void MergeSort(List<Appointment> appointments)
    {
        if (appointments.Count <= 1)
            return;

        var mid = appointments.Count / 2;
        var left = new List<Appointment>(appointments.GetRange(0, mid));
        var right = new List<Appointment>(appointments.GetRange(mid, appointments.Count - mid));

        MergeSort(left);
        MergeSort(right);

        appointments.Clear();
        int i = 0, j = 0;
        while (i < left.Count && j < right.Count)
        {
            if (left[i].AppointmentTime <= right[j].AppointmentTime)
                appointments.Add(left[i++]);
            else
                appointments.Add(right[j++]);
        }

        while (i < left.Count)
            appointments.Add(left[i++]);

        while (j < right.Count)
            appointments.Add(right[j++]);
    }

    // Quick Sort
    public static void QuickSort(List<Appointment> appointments, int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(appointments, low, high);
            QuickSort(appointments, low, pi - 1);
            QuickSort(appointments, pi + 1, high);
        }
    }

    private static int Partition(List<Appointment> appointments, int low, int high)
    {
        var pivot = appointments[high];
        int i = (low - 1);

        for (int j = low; j < high; j++)
        {
            if (appointments[j].AppointmentTime <= pivot.AppointmentTime)
            {
                i++;
                var temp = appointments[i];
                appointments[i] = appointments[j];
                appointments[j] = temp;
            }
        }

        var swap = appointments[i + 1];
        appointments[i + 1] = appointments[high];
        appointments[high] = swap;

        return i + 1;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to Healthcare Appointment System\n");
        // Sample data for doctors and rooms
        List<Doctor> doctors = new List<Doctor>
        {
            new Doctor("Dr. John Doe", "D001", "Cardiologist"),
            new Doctor("Dr. Jane Smith", "D002", "Neurologist"),
            new Doctor("Dr. Emily Davis", "D003", "Dermatologist"),
            new Doctor("Dr. Michael Brown", "D004", "Orthopedist")
        };

        List<Room> rooms = new List<Room>
        {
            new Room("Room 101", "R001", "General"),
            new Room("Room 102", "R002", "Emergency"),
            new Room("Room 201", "R003", "Surgery"),
            new Room("Room 202", "R004", "Recovery")
        };

        // Add appointments
        List<Appointment> appointments = new List<Appointment>();

        Console.WriteLine("Choose Doctor from the following list:");
        for (int i = 0; i < doctors.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {doctors[i].Name} (Specialty: {doctors[i].Specialty})");
        }
        int doctorChoice = int.Parse(Console.ReadLine()) - 1;

        Console.WriteLine("Enter number of appointments:");
        int appointmentCount = int.Parse(Console.ReadLine());
        for (int i = 0; i < appointmentCount; i++)
        {
            Console.WriteLine($"Enter details for Appointment {i + 1}:");
            Console.Write("Patient Name: ");
            string patientName = Console.ReadLine();
            Console.Write("Appointment Time (yyyy-mm-dd hh:mm): ");
            DateTime appointmentTime = DateTime.Parse(Console.ReadLine());
            Console.Write("Priority (1 for Urgent, 2 for Normal): ");
            int priority = int.Parse(Console.ReadLine());

            Console.WriteLine("Choose Room from the following list:");
            for (int j = 0; j < rooms.Count; j++)
            {
                Console.WriteLine($"{j + 1}. {rooms[j].Name} (Type: {rooms[j].RoomType})");
            }
            int roomChoice = int.Parse(Console.ReadLine()) - 1;

            appointments.Add(new Appointment(patientName, appointmentTime, priority, rooms[roomChoice]));
        }

        // Sorting Algorithms Execution
        Console.WriteLine("\nAppointments before sorting:");
        foreach (var appt in appointments)
        {
            Console.WriteLine($"{appt.PatientName} at {appt.AppointmentTime}, Priority {appt.Priority}, Room: {appt.Room.Name}");
        }

        // Bubble Sort and Measure Time - Sahipiramigan N.
        Stopwatch stopwatch = Stopwatch.StartNew();
        SortingAlgorithms.BubbleSort(appointments);
        stopwatch.Stop();
        Console.WriteLine("\nAppointments after Bubble Sort:");
        foreach (var appt in appointments)
        {
            Console.WriteLine($"{appt.PatientName} at {appt.AppointmentTime}, Priority {appt.Priority}, Room: {appt.Room.Name}");
        }
        Console.WriteLine($"Bubble Sort execution time: {stopwatch.ElapsedMilliseconds} ms");

        // Merge Sort and Measure Time - Hasith Heshika
        stopwatch.Restart();
        SortingAlgorithms.MergeSort(appointments);
        stopwatch.Stop();
        Console.WriteLine("\nAppointments after Merge Sort:");
        foreach (var appt in appointments)
        {
            Console.WriteLine($"{appt.PatientName} at {appt.AppointmentTime}, Priority {appt.Priority}, Room: {appt.Room.Name}");
        }
        Console.WriteLine($"Merge Sort execution time: {stopwatch.ElapsedMilliseconds} ms");

        // Quick Sort and Measure Time - Sadeep Sitharaka
        stopwatch.Restart();
        SortingAlgorithms.QuickSort(appointments, 0, appointments.Count - 1);
        stopwatch.Stop();
        Console.WriteLine("\nAppointments after Quick Sort:");
        foreach (var appt in appointments)
        {
            Console.WriteLine($"{appt.PatientName} at {appt.AppointmentTime}, Priority {appt.Priority}, Room: {appt.Room.Name}");
        }
        Console.WriteLine($"Quick Sort execution time: {stopwatch.ElapsedMilliseconds} ms");
    }
}