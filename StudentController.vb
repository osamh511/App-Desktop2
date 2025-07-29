Imports LAERN_USE.BLL.Factories
Imports LAERN_USE.BLL.Interface
Imports LAERN_USE.DomainModels
Imports LAERN_USE.BLL.Enums.StorageMode ' تم حذ الكلاس 
Imports LAERN_USE.BLL.Enums
Namespace BLL.Controller '6️⃣ طبقة التوصيل StudentController
    '''مــــلأحظة
    '''📍 2. الكائن يحتوي بياناته داخليًا ← يتم إنشاؤه داخل Controller بخصائص كاملة
    '''    🧠 التحليل:
    '''المدخلات تأتي من الواجهة (Form1)
    '''يتم تمريرها إلى StudentController عبر AddStudent(...)
    '''داخل Controller يتم إنشاء الكائن STUDENT4 باستخدام كل خصائصه (الهوية، الاسم، العمر)
    '''الكائن يُرسل بعد ذلك إلى الخدمة كـ وحدة كاملة مستقلة
    '''↪️ لا يتم استخدام دوال مثل Create(name) أو Print("اسم الطالب") ← بل نمرر الكائن نفسه ليقوم النظام بمعالجته ↪️ هذا يحافظ على العقد الداخلي للكائن، ويمنع الاعتماد على مدخلات خارجية


    'طبقة التوصيل StudentController تعتبر وسيط بيط الواجهه والسيرفرس 
    'UI
    'StudentService
    Public Class StudentController '🧠 يُمثّل المعالج الرئيسي الذي يستقبل الطلبات من الواجهة
        Private ReadOnly _service As IStudentService
        'الخطوة الثاني Adapter
        '''        🔎 معنى هذا:
        '''يتم تمرير نوع التخزين إلى StudentServiceFactory
        '''المصنع يحدد التنفيذ المناسب ← هذا هو نقطة الحقن الرئيسية (Factory + Bridge)

        '🧠 ملخص التدفق في Adapter:
        '''Form1 → Factory → StudentService → LegacyStorageAdapter → LegacyStorage

        '  الخطوة الثاني. Bridge Pattern ⇄ DIP + OCP
        '''🔎 ماذا يحدث؟
        '''يتم تمرير نوع التخزين المختار إلى Factory
        '''ويُعاد كائن StudentService 
        '''يحتوي التنفيذ الصحيح ← هذا هو الجسر
        '''

        '🧠 ملخص التدفق في Bridge:
        '''Form1 → StudentController → StudentService ← (Bridge)
        '''                                    ↑
        '''                    [Csv / Legacy] IStudentRepository

        Public Sub New(mode As StorageMode)
            _service = StudentServiceFactory.Create(mode)
        End Sub
        ''' <summary>
        ''' يتم إنشاء الكائن STUDENT4 باستخدام كل خصائصه
        ''' 🧪 التدفق الخوارزمي خطوة بخطوة:
        '''المرحلة	ما يحدث
        '''1️⃣ الـ Form1 يجمع المدخلات من txtId.Text, txtName.Text, txtAge.Text
        '''2️⃣ يرسل القيم عبر AddStudent(id, name, age) إلى الـ Controller
        '''3️⃣ يتم بناء كائن STUDENT4 داخل Controller باستخدام ثلاث خصائص مباشرة
        '''4️⃣ الكائن جاهز — يحتوي على كل بياناته داخليًا (ذاتي الإنشاء)
        ''' </summary>
        ''' <param name="id">خاصية الهوية</param>
        ''' <param name="name">خاصية الاسم</param>
        ''' <param name="age">خاصية العمر</param>

        '🔧 هذا يضمن أن الكائن مكتمل ذاتيًا، ولا ينتظر مدخلات لاحقة لتكميل سلوكه
        Public Sub AddStudent(id As Integer, name As String, age As Integer)
            '🔧 يتم ربط قيمة name المرسلة من Form1 بخاصية Name داخل الكائن —
            'وهكذا يتم "حقن البيانات" داخليًا، ولا تعتمد على مدخلات لاحقة.
            Dim student As New STUDENT4 With {.Id = id, .Name = name, .Age = age} 'يتم بناء الكائن STUDENT4 عبر هذا السطر
            _service.Create(student) ' يتم إرسال الكائن إلى StudentService عبر الواجهة ← دون معرفة تفاصيل التنفيذ


            '        _service.Create(student) 💡 ما يحدث هنا
            'المرحلة التفسير
            '✅ student هو نسخة مكتملة من STUDENT4
            '✅ يتم تمريره إلى الخدمة عبر واجهة IStudentService.Create(student)
            '✅ الخدمة لا تطلب أي معلومات إضافية
            '✅ الكائن يُعامل كوحدة تنفيذ مستقلة — وليس مجرد تجميع بيانات
            '↪️ هذا يحقق مبدأ LSP لأن النظام يتعامل مع الكائن عبر واجهة موحدة، ولا يهتم بنوعه أو طريقة إنشائه
        End Sub
        ' مــــلاحظة شكل العام 
        '        ✅ 4. الاستدعاء والاستخدام ← تشغيل الكائن المحقون
        '📌 الفكرة: تنفيذ العمليات(مثل GetAllStudents) باستخدام الكائن الذي تم إنشاؤه
        '            يتم استخدام _service ← الذي تم إنشاؤه عبر Factory
        'العملية تتم بشكل كامل دون معرفة تفاصيل التخزين
        '            الكائن المحقون هو StudentService
        'تم إنشاؤه مسبقًا عبر StudentServiceFactory.Create(mode)
        'وتم تخزينه داخل _service في نفس الكونترولر
        'والدوال مثل GetAllStudents() و AddStudent() تستدعي وظائف الخدمة مباشرة
        '✅ هذا يعني أن الاستخدام يتم داخل طبقة Controller ← وهي طبقة التوصيل بين واجهة المستخدم والطبقة المنطقية.

        ''' <summary>
        '''         Public Function GetAllStudents() As List(Of STUDENT4) في الدالة
        ''' تعود قائمة طلاب ← تُستخدم داخل
        ''' Form1
        ''' لعرضها في DataGridView مثلاً 
        ''' </summary>
        ''' <returns></returns>
        Public Function GetAllStudents() As List(Of STUDENT4)
            'Factory Method
            'تعتمد على كائن StudentService
            'مخزّن في الحقل _service
            '  الدالة نفسها لا تهتم بكيف تم إنشاء
            '  _service
            '  أو كيف تم إنشاء التخزين خلف الكواليس
            'هذا يعني إن الإنشاء تم سابقًا في مكان آخر
            Return _service.GetAllStudents()
        End Function


    End Class
End Namespace

