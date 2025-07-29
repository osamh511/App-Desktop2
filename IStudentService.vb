Imports LAERN_USE.DomainModels
Namespace BLL.Interface
    Public Interface IStudentService '🧠 هذه العقود تُستخدم لضمان التوافق بين الطبقات وتطبيق DIP
        Sub Create(student As STUDENT4)
        Function Read(id As Integer) As STUDENT4
        Sub Update(student As STUDENT4)
        Sub Delete(id As Integer)
        Function GetAllStudents() As List(Of STUDENT4)
    End Interface
End Namespace


