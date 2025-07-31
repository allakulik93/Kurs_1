using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// Интерфейс для работы с абонентами
public interface ISubscriberService
{
    void AddSubscriber(int id, string name, string phoneNumber, string address);
    void RemoveSubscriber(int id);
    Subscriber FindSubscriber(int id);
    void ShowAllSubscribers();
}

// Базовый класс абонента
public abstract class SubscriberBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }

    // Конструктор
    public SubscriberBase(int id, string name, string phoneNumber, string address)
    {
        Id = id;
        Name = name;
        PhoneNumber = phoneNumber;
        Address = address;
    }

    // Метод ToString для вывода информации
    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Phone: {PhoneNumber}, Address: {Address}";
    }

    // Абстрактный метод для выполнения дополнительных действий (может быть переопределён в потомках)
    public abstract void DisplaySubscriberInfo();
}

// Класс для абонента, наследующий от SubscriberBase
public class Subscriber : SubscriberBase
{
    // Дополнительные свойства или методы могут быть добавлены здесь
    public Subscriber(int id, string name, string phoneNumber, string address)
        : base(id, name, phoneNumber, address)
    { }

    // Переопределение метода для отображения информации об абоненте
    public override void DisplaySubscriberInfo()
    {
        Console.WriteLine(this.ToString());
    }
}

// Класс для работы с абонентами, реализующий интерфейс ISubscriberService
public class SubscriberService : ISubscriberService
{
    private List<SubscriberBase> subscribers = new List<SubscriberBase>();

    // Метод для добавления абонента
    public void AddSubscriber(int id, string name, string phoneNumber, string address)
    {
        var newSubscriber = new Subscriber(id, name, phoneNumber, address);
        subscribers.Add(newSubscriber);
        Console.WriteLine($"Абонент {name} добавлен успешно!");
    }

    // Метод для удаления абонента по ID
    public void RemoveSubscriber(int id)
    {
        var subscriber = subscribers.Find(s => s.Id == id);
        if (subscriber != null)
        {
            subscribers.Remove(subscriber);
            Console.WriteLine($"Абонент с ID {id} удалён.");
        }
        else
        {
            Console.WriteLine("Абонент с таким ID не найден.");
        }
    }

    // Метод для поиска абонента по ID
    public Subscriber FindSubscriber(int id)
    {
        return (Subscriber)subscribers.Find(s => s.Id == id);
    }

    // Метод для вывода всех абонентов
    public void ShowAllSubscribers()
    {
        if (subscribers.Count == 0)
        {
            Console.WriteLine("Нет абонентов.");
            return;
        }

        foreach (var subscriber in subscribers)
        {
            subscriber.DisplaySubscriberInfo();
        }
    }
}

// Основной класс программы
public class Program
{
    static void Main()
    {
        ISubscriberService service = new SubscriberService();

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Добавить абонента");
            Console.WriteLine("2. Удалить абонента");
            Console.WriteLine("3. Найти абонента по ID");
            Console.WriteLine("4. Показать всех абонентов");
            Console.WriteLine("5. Выход");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    try
                    {
                        Console.Write("Введите ID абонента: ");
                        int id = int.Parse(Console.ReadLine());  // Может вызвать исключение при неправильном вводе

                        Console.Write("Введите имя абонента: ");
                        string name = Console.ReadLine();

                        // Проверка, чтобы имя не содержало цифр
                        if (ContainsDigits(name))
                        {
                            throw new FormatException("Имя не должно содержать цифры.");
                        }

                        Console.Write("Введите номер телефона абонента: ");
                        string phoneNumber = Console.ReadLine();

                        // Проверка корректности ввода номера телефона
                        if (!IsValidPhoneNumber(phoneNumber))
                        {
                            throw new FormatException("Номер телефона должен содержать только цифры и начинаться с +.");
                        }

                        Console.Write("Введите адрес абонента: ");
                        string address = Console.ReadLine();

                        // Проверка корректности адреса
                        if (!IsValidAddress(address))
                        {
                            throw new FormatException("Адрес должен содержать только буквы и цифры, и начинаться с буквы.");
                        }

                        service.AddSubscriber(id, name, phoneNumber, address);
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
                    }
                    break;

                case "2":
                    try
                    {
                        Console.Write("Введите ID абонента для удаления: ");
                        int id = int.Parse(Console.ReadLine());
                        service.RemoveSubscriber(id);
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;

                case "3":
                    try
                    {
                        Console.Write("Введите ID абонента для поиска: ");
                        int id = int.Parse(Console.ReadLine());
                        var subscriber = service.FindSubscriber(id);
                        if (subscriber != null)
                        {
                            Console.WriteLine(subscriber);
                        }
                        else
                        {
                            Console.WriteLine("Абонент с таким ID не найден.");
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;

                case "4":
                    service.ShowAllSubscribers();
                    break;

                case "5":
                    Console.WriteLine("Выход из программы.");
                    return;

                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    // Метод для проверки корректности номера телефона
    static bool IsValidPhoneNumber(string phoneNumber)
    {
        // Регулярное выражение для проверки формата телефона: например, +79001234567
        string pattern = @"^\+?\d{7,15}$";  // От 7 до 15 цифр, возможно, с плюсом в начале
        return Regex.IsMatch(phoneNumber, pattern);
    }

    // Метод для проверки, содержит ли строка цифры
    static bool ContainsDigits(string input)
    {
        return Regex.IsMatch(input, @"\d");
    }

    // Метод для проверки корректности адреса
    static bool IsValidAddress(string address)
    {
        // Адрес должен начинаться с буквы, содержать буквы, цифры и пробелы
        string pattern = @"^[А-Яа-яA-Za-z][А-Яа-яA-Za-z0-9,.]*$";
        return Regex.IsMatch(address, pattern);
    }
}

