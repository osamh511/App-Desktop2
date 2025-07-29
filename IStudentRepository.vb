
Imports LAERN_USE.DomainModels

Namespace BLL.Interface 'العقد الموحد
    'هنا تحديدا تم استخدام نمط 🧱 Repository Pattern

    '2. 🔹 واجهة التخزين – داخل طبقة BLL
    '    🧑‍💻 برمجياً:
    'واجهة تحدّد التوقيع العام لكل عملية من CRUD
    '🧠 خوارزمياً:
    'BLL يتعامل مع هذه الواجهة فقط دون معرفة تفاصيل التنفيذ — وهذا هو DIP بعينه

    ''' <summary>
    ''' 🧠 إذًا ما هو دور الواجهة IStudentRepository؟
    '''الواجهة تُستخدم من أجل:
    '''تحديد التوقيع العام للعمليات: Create, Read, Update, Delete
    '''تطبيق مبدأ DIP: تعتمد BLL على هذه الواجهة وليس على تنفيذ محدد
    '''تحقيق مبدأ OCP: يمكنك إضافة أنواع تخزين جديدة مثل SqlRepository أو OracleRepository دون تعديل كود StudentService
    '''✅ يعني:
    '''IStudentRepository ليست "طبقة المعالجة"، بل هي العقد بين المعالجة والتخزين
    Public Interface IStudentRepository '🧠 هذه العقود تُستخدم لضمان التوافق بين الطبقات وتطبيق DIP
        Sub Create(student As STUDENT4)
        Function Read(id As Integer) As STUDENT4
        Sub Update(student As STUDENT4)
        Sub Delete(id As Integer)
        Function GetAll() As List(Of STUDENT4)


    End Interface
End Namespace

