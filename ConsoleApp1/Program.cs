using System.Data;
using System.Data.SqlClient;
using Classes;
using System;

//массивы объектов классов работников, департаментов, начальников
EmployeeClass[] employee = new EmployeeClass[7];
DepartmentClass[] department = new DepartmentClass[3];
ChiefClass[] chief = new ChiefClass[7];

//путь к базе состоит из пути к исполняющему приложению + имя базы
string basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Database1.mdf";

//строка подключения к базе
string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + basePath + "; Integrated Security = True";
SqlConnection sqlConnection = new SqlConnection(connectionString);

//метод покдлючения к базе
void openConnection()
    {
    try
    {
        if (sqlConnection.State == System.Data.ConnectionState.Closed)
        {
            sqlConnection.Open();
        }
    }

    catch (Exception ex)
    {
        Console.WriteLine("Ошибка подключения к базе данных:");
        Console.WriteLine(ex.Message);
        Console.WriteLine("Нажмите любую клавишу для завершения программы");
        Console.ReadLine();
        Environment.Exit(1);
    } 
    }

//метод отключения от базы
void closeConnection()
    {
        if (sqlConnection.State == System.Data.ConnectionState.Open)
        {
            sqlConnection.Close();
        }
    }

openConnection();

//Вывод сообщения об успешном подключении к базе
if (sqlConnection.State == ConnectionState.Open)
{
    Console.WriteLine("Подключение к базе данных успешно");
    Console.WriteLine();
}

//переменная для хранения считываемых строк таблиц
SqlDataReader? dataReader = null;

//счетчик работников
int empCounter = 0;

//заполнение элементов класса employee[] - массива работников
try
{
    SqlCommand sqlCommand = new SqlCommand("SELECT id, department_id, chief_id, name, salary FROM employee", sqlConnection);
    dataReader = sqlCommand.ExecuteReader();

    //вывод шапки таблицы считываемых данных
    Console.WriteLine("Выводим список работников: ");
    Console.WriteLine("ИД\t\t Департамент\t ИД Руководителя\t Имя\t Зарплата ");

    //построчно считываем данные
    while (dataReader.Read())
    {
        //инициализция каждого элемента массива из класса
        employee[empCounter] = new EmployeeClass();

        employee[empCounter].id = Convert.ToInt32(dataReader["id"]);
        employee[empCounter].department_id = Convert.ToInt32(dataReader["department_id"]);

        // при встрече пустого значения в графе "Руководитель" конвертируем в текст NULL
        if (Convert.ToString(dataReader["chief_id"]) == "") 
        {
            employee[empCounter].chief_id = "NULL";
        }
        else
        {
             employee[empCounter].chief_id = Convert.ToString(dataReader["chief_id"]);
        }
        employee[empCounter].name = Convert.ToString(dataReader["name"]);
        employee[empCounter].salary = Convert.ToInt32(dataReader["salary"]);

        //вывод считанных данных из таблицы по каждому рабонику
        Console.WriteLine($"{employee[empCounter].id}\t\t{employee[empCounter].department_id}\t\t{employee[empCounter].chief_id}\t\t\t{employee[empCounter].name}\t{employee[empCounter].salary}");
        empCounter++;

    }
}

//если чтение из таблицы прошло с ошибками - вывод теста ошибки
catch (Exception ex)
{
    Console.WriteLine("Ошибка подключения к таблице работников:");
    Console.WriteLine(ex.Message);
    Console.WriteLine("Нажмите любую клавишу для завершения программы");
    Console.ReadLine();
    Environment.Exit(1);
}
finally
{

    //если строка прочитанных данных не пустая или не закрыта, то закрываем модуль чтения
    if (dataReader != null && !dataReader.IsClosed)
    {
        dataReader.Close();
    }
    //прекратить работу программы
}

// счетчик департаментов
int depCounter = 0;

//заполнение элементов класса department[] - массива департаментов
try
{
    SqlCommand sqlCommand = new SqlCommand("SELECT id, name FROM department", sqlConnection);
    dataReader = sqlCommand.ExecuteReader();

    //построчно считываем данные и заносим в элементы класса
    while (dataReader.Read())
    {
        department[depCounter] = new DepartmentClass();
        department[depCounter].id = Convert.ToInt32(dataReader["id"]);
        department[depCounter].name = Convert.ToString(dataReader["name"]);
        depCounter++;
    }
}

//если чтение из таблицы прошло с ошибками - вывод теста ошибки
catch (Exception ex)
{
    Console.WriteLine("Ошибка подключения к таблице департаментов:");
    Console.WriteLine(ex.Message);
    Console.WriteLine("Нажмите любую клавишу для завершения программы");
    Console.ReadLine();
    Environment.Exit(1);
    }
finally
{

    //если строка прочитанных данных не пустая или не закрыта, то закрываем модуль чтения
    if (dataReader != null && !dataReader.IsClosed)
    {
        dataReader.Close();
    }
}

//вывод справочной информации
Console.WriteLine();
Console.WriteLine("Введите номер команды:");
Console.WriteLine("1 - Сумм зарплат работинков со всех департаментов без руководителей");
Console.WriteLine("2 - Сумма зарплат работинков со всех департаментов с руководителями");
Console.WriteLine("3 - Департамент, в котором у сотрудника зарплата максимальна");
Console.WriteLine("4 - Зарплаты руководителей по убыванию");
Console.WriteLine("0 - Выход из программы");
Console.WriteLine();
Console.Write("Номер команды: ");

//переменная для хранения строки ввода
string? input="";

//метод проверки ввода
void isNumber()
{
    input = Console.ReadLine();
    Console.WriteLine();

    //если ввели пустую строку или текст - повторный вызов функции проверки ввода
    while ((int.TryParse(input, out int output) == false) || (input == ""))
    {
        Console.WriteLine("Команда не существует. Повторите ввод");
        Console.Write("Номер команды: ");
        isNumber();
    }

    //если ввели номер команды, отличный от предложенных - повторный вызов функции проверки ввода
    while ((Convert.ToInt32(input) != 1) && (Convert.ToInt32(input) != 2) && (Convert.ToInt32(input) != 3) && (Convert.ToInt32(input) != 4) && (Convert.ToInt32(input) != 0)) //проверка на соотвуствие команды
    {
        Console.WriteLine("Команда не существует. Повторите ввод");
        Console.Write("Номер команды: ");
        isNumber();
    }
}

//запуск ввода и проверки ввода
isNumber();

//переменная для хранения номера команды
int che_nado = Convert.ToInt32(input);

//обнуляем массив руководителей
for (int i = 0; i < empCounter; i++)
{
    chief[i] = new ChiefClass();
    chief[i].id = 0;
    chief[i].dep_id = 0;
    chief[i].salary = 0;
}

// ищем руководителей и и заполняем элементы класса руководителей
for (int i = 0; i < empCounter; i++) 
    for (int j = 0; j < empCounter; j++)

        //для каждого работника сравниваем id его руководителя с id другого работника. При нахождении  такого id работника, его данные попадают в элемент класса руководителей
        if (Convert.ToString(employee[i].id) == employee[j].chief_id)
        {
            chief[i].id = employee[i].id;
            chief[i].dep_id = employee[j].department_id;
            chief[i].salary = employee[i].salary;
        }

//массив для хранения максимальных зарплат по департаменту без/с руководителями
int[] sum_dep = new int[depCounter];

//переменная для хранения максимальной зарплаты по департаменту
int max_zp = 0;

//переменная для хранения названия департамента с макс запрлатой
string? max_zp_dep = "";


switch (che_nado)
{

    //если выбрана задача найти суммарную зарплату по департаментам без руководителей
    case 1:

        Console.WriteLine("Суммарная зарплата по департаментам без руководителей");
        
        //для каждого департамента
        for (int i = 0; i < depCounter; i++)
        {
            sum_dep[i] = 0;
            for (int j = 0; j < empCounter; j++)
            {

                //ищем совпадание id департамента c id департамента работника, но без совпадения id департамента с id департамента руководителя
                if ((department[i].id == employee[j].department_id) && (department[i].id != chief[j].dep_id))
                    sum_dep[i] += employee[j].salary;
            }
        Console.WriteLine($"По {department[i].name} равна {sum_dep[i]} рублей");
        }

        closeConnection();
        break;

    //если выбрана задача найти суммарную зарплату по департаментам без руководителей
    case 2:
            Console.WriteLine("Суммарная зарплата по департаментам с руководителями");

        //для каждого департамента      
        for (int i = 0; i < depCounter; i++)
            {
                sum_dep[i] = 0;
                for (int j = 0; j < empCounter; j++)
                {

                //ищем совпадание id департамента c id департамента работника или совпадения id департамента с id департамента руководителя
                if ((department[i].id == employee[j].department_id) || (department[i].id == chief[j].dep_id))
                        sum_dep[i] += employee[j].salary;
                }
            Console.WriteLine($"По {department[i].name} равна {sum_dep[i]} рублей");
            }

        closeConnection();
        break;
        
      //если выбрана задача нахождения максимальной зарплаты и вывода название департамента, который принадлежит работник с этой зп
      case 3:

            //проходим по всем работникам
            for (int i = 0; i < empCounter; i++)

                //если зп текущего работника меньше масимальной, то сохраняем эту зп и название департамента
                if ((Convert.ToInt32(employee[i].salary)) > max_zp)
                {
                    max_zp = Convert.ToInt32(employee[i].salary);

                //-1 индекс, т.к. нумерация в массиве начинается с 0, а идентификаторы с 1
                max_zp_dep = department[Convert.ToInt32(employee[i].department_id) - 1].name; 
                }
            Console.WriteLine($"Департамент, в котором у сотрудника зарплата максимальна - {max_zp_dep}, ее значение = {max_zp}");
        
        closeConnection();
        break;
        
      //если выбрана задача вывода зарплат руководителей по убыванию
      case 4:

        //переменная для хранения количства руководителей
        int chiefVol = 0;

        //вычисляем количество руководителей
        for (int i = 0; i < empCounter; i++)
        {
            if (chief[i].id != 0) chiefVol++;
        }

        //объявляем массив для хранения зарплат руководителей
        int[] salaryChief = new int[chiefVol];

        //счетчик руководителей
        int chiefCounter = 0;

        //заносим зп руководителей в массив salaryChief[]
        for (int i = 0; i < empCounter; i++) 
                if (chief[i].salary != 0)
                {
                    salaryChief[chiefCounter] = chief[i].salary;
                    chiefCounter++;
                }

        //сортируем массив руководителей по убыванию, меняя местами соседние эелементы
        for (int i = 1; i < salaryChief.GetUpperBound(0) + 1; i++)
                for (int j = i; j > 0; j--)
                    if (salaryChief[j] > salaryChief[j - 1])
                    {
                        int temp = salaryChief[j - 1];
                        salaryChief[j - 1] = salaryChief[j];
                        salaryChief[j] = temp;
                    }

        Console.WriteLine($"Сортируем зарплаты руководителей по убыванию:");

        //выводим отсортированный массив
        for (int i = 0; i < salaryChief.GetUpperBound(0) + 1; i++)
                Console.Write($" {salaryChief[i]} ");
        closeConnection();
        break;
  
    default:
        break;
}
Console.WriteLine();
Console.WriteLine("Нажмите любую клавишу для завершения программы");
Console.ReadLine();