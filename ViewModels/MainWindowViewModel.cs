using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using projekt_Jakub_Siwek_Z403_AV.Data;
using projekt_Jakub_Siwek_Z403_AV.Models;

namespace projekt_Jakub_Siwek_Z403_AV.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();

        [ObservableProperty]
        private string _nameInput = string.Empty;

        [ObservableProperty]
        private string _surnameInput = string.Empty;

        [ObservableProperty]
        private string _groupInput = string.Empty;

        [ObservableProperty]
        private string _statsOutput = "Inicjalizacja...";

        [ObservableProperty]
        private Student? _selectedStudent;

        [ObservableProperty]
        private string _statusMessage = "Dzień dobry";

        [ObservableProperty]
        private string _statusColor = "#CCCCCC";

        [ObservableProperty]
        private string _statusIcon = "ℹ️";

        public void ShowAlert(string message, string hexColor, string icon)
        {
            StatusMessage = message;
            StatusColor = hexColor;
            StatusIcon = icon;
        }

        // As odczyt z bazy
        public async Task LoadAsync()
        {
            try
            {
                using var db = new AppDbContext();
                var data = await db.Students.ToListAsync();

                Students.Clear();
                foreach (var student in data)
                {
                    Students.Add(student);
                }

                UpdateStatsParallel(data);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd bazy: {ex}");
                ShowAlert("Brak połączenia z LocalDB SQL Server!", "#EF4444", "❌");
            }
        }

        // Równ
        private void UpdateStatsParallel(List<Student> studentList)
        {
            if (studentList.Count == 0)
            {
                StatsOutput = "Brak rekordów";
                return;
            }

            Task.Run(() =>
            {
                int totalCount = studentList.Count;
                int longNamesCount = 0;

                Parallel.ForEach(studentList, student =>
                {
                    if (!string.IsNullOrEmpty(student.Name) && student.Name.Length > 4)
                    {
                        System.Threading.Interlocked.Increment(ref longNamesCount);
                    }
                });

                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    StatsOutput = $"Wszyscy: {totalCount} | Imiona > 4 znaków: {longNamesCount}";
                });
            });
        }

        //  AS (Zółte i zielone powiadomienia)
        public async Task AddStudentAsync()
        {
            //  ŻÓŁTA OBRAMÓWKA
            if (string.IsNullOrWhiteSpace(NameInput) || string.IsNullOrWhiteSpace(SurnameInput) || string.IsNullOrWhiteSpace(GroupInput))
            {
                ShowAlert("Brak danych do dodania studenta", "#EAB308", "⚠️"); // Żółty
                return;
            }

            try
            {
                using var db = new AppDbContext();
                db.Students.Add(new Student
                {
                    Name = NameInput,
                    Surname = SurnameInput,
                    GroupName = GroupInput
                });

                await db.SaveChangesAsync();

                // Sukces -> ZIELONA OBRAMÓWKA
                ShowAlert("Prawidłowo dodano studenta", "#22C55E", "✔️"); // Zielony hex

                await LoadAsync();

                NameInput = string.Empty;
                SurnameInput = string.Empty;
                GroupInput = string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd dodawania: {ex}");
            }
        }

        // ELEMENT 1: AS (Zielone powiadomienie)
        public async Task DeleteStudentAsync()
        {
            if (SelectedStudent == null)
            {
                ShowAlert("Zaznacz studenta z listy do usunięcia!", "#EAB308", "⚠️");
                return;
            }

            try
            {
                using var db = new AppDbContext();
                var studentInDb = await db.Students.FindAsync(SelectedStudent.Id);
                if (studentInDb != null)
                {
                    db.Students.Remove(studentInDb);
                    await db.SaveChangesAsync();

                    // ZIELONA OBRAMÓWKA
                    ShowAlert("Prawidłowo usunięto studenta", "#22C55E", "✔️");
                }
                await LoadAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd usuwania: {ex}");
            }
        }
    }
}