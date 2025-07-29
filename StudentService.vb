Imports LAERN_USE.DomainModels
Imports LAERN_USE.BLL.Interface
'''    🔸 2. طبقة BLL (Business Logic Layer)
'''✅ هنا تُكتب الكود الفعلي للمنطق: مثل:
'''vb.net
'''Dim student As New Student3("علي", 22)
'''StudentService.Register(student)
'''📌 وهي فعليًا اللي:
'''تستقبل الكائنات من UI
'''تنفّذ منطق الفحص والتسجيل
'''تستدعي Repository المناسب
'''🎯 هذه هي الطبقة الرئيسية لتطبيق CRUD من منظور الباك اند
Namespace BLL.Services ' هنا اصبح فقط مركز على المنطق الاساسي فقط
    ' بحيث يعتمد علىIStudentRepository للتعامل مع البيانات عن طريق الحقن جواة لكي يتعامل مع البيانات (DI)
    ' وكذالك يعتمد على 
    '

    '4. 🔹 استخدام داخل BLL
    '        🧑‍💻 برمجياً:
    'الكلاس StudentService يتعامل فقط مع الواجهة، ويخفي كل التفاصيل
    '🧠 خوارزمياً:
    'الخدمة تعمل كوسيط بين الواجهة وبين التخزين، وتنفّذ المنطق المطلوب


    Public Class StudentService '🧠 يحتوي منطق الأعمال ويستند فقط إلى الواجهة، مما يحقق DIP
        Implements IStudentService

        Private ReadOnly _repo As IStudentRepository 'يعتمد على IStudentRepository ← تنفيذ موحّد
        'مــــلأحظة الشكل العام
        '        ✅ 3. الحقن عبر الـ Constructor ← إنشاء كامل بالواجهة
        '📌 الفكرة: تمرير التنفيذ إلى الخدمة بطريقة آمنة ومحترمة لمبدأ DIP
        '            يتم الاحتفاظ بالتنفيذ داخل _repo
        'هذا هو الحقن المعماري بالواجهة

        '' 🧠 حقن التبعية عبر الواجهة
        ''🔹 الإنشاء الكامل بالحقن	
        Public Sub New(repo As IStudentRepository)
            _repo = repo
        End Sub

        ' 🟢 إضافة طالب جديد
        ''' <summary>
        '''      هنا يتم تطبيق نمط🧱 1. Repository Pattern ⇄ DIP + SRP 
        '''        🔎 ماذا يحدث؟
        '''StudentService يستدعي Create() من الواجهة IStudentRepository
        '''لا يعرف ما إذا كانت العملية تتم على CSV أو Legacy
        '''✔️ هذا يحقق DIP: الخدمة تعتمد على واجهة تجريدية وليس تنفيذ محدد


        '''         '''     🧠 ملخص التدفق في Repository:
        '''     Form1 → StudentController → StudentService → IStudentRepository → CsvStudentRepository


        ''' هنا يتم تطبيق مبدى 🔄 3. Bridge Pattern ⇄ DIP + OCP
        '''الهدف: فصل التجريد (مثل StudentService)
        '''عن التنفيذ الحقيقي
        '''(نوع التخزين)، 
        '''بحيث يمكن تبديل التخزين دون تعديل الكود
        '''
        '🧠 ملخص التدفق في Bridge:
        '''Form1 → StudentController → StudentService ← (Bridge)
        '''                                    ↑
        '''                    [Csv / Legacy] IStudentRepository
        Public Sub Create(student As STUDENT4) Implements IStudentService.Create
            ' ممكن إضافة تحقق من البيانات لاحقًا هنا
            _repo.Create(student) ' التنفيذ يعتمد على الواجهة
        End Sub

        ' 🟢 جلب طالب واحد حسب ID
        Public Function Read(id As Integer) As STUDENT4 Implements IStudentService.Read
            Return _repo.Read(id)
        End Function

        ' 🟢 تحديث بيانات طالب
        Public Sub Update(student As STUDENT4) Implements IStudentService.Update
            _repo.Update(student)
        End Sub

        ' 🟢 حذف طالب
        Public Sub Delete(id As Integer) Implements IStudentService.Delete
            _repo.Delete(id)
        End Sub

        '' 🟢 جلب جميع الطلاب

        ' الخطوة الثاني لنمط🧱 1. Repository Pattern ⇄ DIP + SRP
        '''        🔎 ماذا يحدث؟
        '''StudentService يستدعي Create
        '''() من الواجهة 
        '''IStudentRepository
        '''لا يعرف ما إذا كانت العملية تتم على CSV أو Legacy
        '''✔️ هذا يحقق DIP: الخدمة تعتمد على واجهة تجريدية وليس تنفيذ محدد

        'ملخص 🧠 ملخص التدفق في Adapter:
        '''Form1 → Factory → StudentService → LegacyStorageAdapter → LegacyStorage

        ' هنا الخطوة الرابع. Bridge Pattern ⇄ DIP + OCP
        '''        🔎 ماذا يحدث؟
        '''StudentService يستدعي GetAll() من الواجهة فقط
        '''التنفيذ الداخلي يتم ضمن الكائن الذي تم حقنه (CSV أو Legacy)

        '🧠 ملخص التدفق في Bridge:
        '''Form1 → StudentController → StudentService ← (Bridge)
        '''                                    ↑
        '''                    [Csv / Legacy] IStudentRepository

        Public Function GetAllStudents() As List(Of STUDENT4) Implements IStudentService.GetAllStudents
            Return _repo.GetAll() '' ← التنفيذ داخل Adapter
        End Function


    End Class
End Namespace

