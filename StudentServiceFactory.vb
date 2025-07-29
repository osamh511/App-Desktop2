Imports LAERN_USE.BLL.Adapter
Imports LAERN_USE.BLL.Enums
Imports LAERN_USE.BLL.Interface
Imports LAERN_USE.BLL.Services
Imports LAERN_USE.DLL
Namespace BLL.Factories '7️⃣ طبقة المصنع StudentServiceFactory
    '📦 من أين تُحقن التبعية؟
    'عبر Factory
    '🔹 الإنشاء الديناميكي مطبق


    '🧑‍💻 برمجياً
    ': تعريف الكلاس الذي ينفذ واجهة التخزين
    '🧠 خوارزمياً
    ': يُخضع نفسه لعقد IStudentRepository
    'ويلتزم بتنفيذ كل عمليات CRUD.
    Public Class StudentServiceFactory '🧠 ينشئ كائن الخدمة مع التبعيات دون كشف التفاصيل، يدعم OCP
        ''' <summary>
        ''' 🎯 هنا:
        '''المصنع أنشأ CsvStudentRepository
        '''ثم حقنه داخل StudentService
        '''وأرجع كائن IStudentService جاهز للعمل
        ''' </summary>
        ''' <returns>واجهة الخدمة الجاهزة للاستخدام</returns>

        'مـــــلاحظة مهمة عامة
        '        ✅ 2. تطبيق دالة المصنع ← الإنشاء الديناميكي حسب السياق
        '📌 الفكرة: إنشاء كائن التخزين المناسب عبر دالة مركزية
        '            يتم اختيار التنفيذ المناسب

        'يتم حقنه في StudentService
        'يعاد ككائن من نوع IStudentService

        ''' <summary>
        '''  
        ''' </summary>
        ''' <param name="mode"> ال يحمل البارمتر الي يحمل السياق العلمية اي اختيرت مثل(CSV,Legacy)</param>
        ''' <param name="StorageMode">هو السياق ← يحدد نوع التنفيذ</param>
        ''' <returns></returns>
        Public Shared Function Create(mode As StorageMode) As IStudentService '← يتم إرجاع الواجهة فقط، وليس التنفيذ المحدد
            '            ✅ المقصود بـ
            'Return As IStudentService ← يتم إرجاع الواجهة فقط، وليس التنفيذ المحدد
            '← يتم إرجاع الواجهة فقط، وليس التنفيذ المحدد
            'هو أن دالة المصنع (StudentServiceFactory.Create)
            'تُعيد كائنًا من نوع
            'IStudentService
            '←
            'والواجهة المجردة لكنها داخليًا تنشئ الكائن الفعلي من نوع
            'StudentService
            '←
            'وينفذ هذه الواجه
            Dim repo As IStudentRepository

            Select Case mode 'المعطى الذي يحدد السياق (CSV أو Legacy)
                Case StorageMode.Csv
                    '  الخطوة الثالث لنمط. Bridge Pattern ⇄ DIP + OCP        
                    ''' 🔎 ماذا يحدث؟
                    '''يتم تحديد التنفيذ بناءً على الاختيار
                    '''ثم حقنه داخل StudentService ← الذي لا يعرف نوع التخزين فعليًا
                    '''✔️ فصل التجريد عن التنفيذ = Bridge ✔️ التبديل دون تعديل الكود = OCP

                    '🧠 ملخص التدفق في Bridge:
                    '''Form1 → StudentController → StudentService ← (Bridge)
                    '''                                    ↑
                    '''                    [Csv / Legacy] IStudentRepository
                    repo = New CsvStudentRepository()
                Case StorageMode.Legacy
                    ' هنا الخطوة الثالثAdapter
                    '''      🔎 معنى هذا
                    '''إذا كان التخزين = Legacy ↪️ يتم إنشاء LegacyStorageAdapter ↪️ وهو كائن ينفذ نفس الواجهة IStudentRepository ↪️ ويُغلف LegacyStudentStorage
                    '''✔️ هنا يتم تطبيق Adapter Pattern بكل وضوح

                    '🧠 ملخص التدفق في Adapter:
                    '''Form1 → Factory → StudentService → LegacyStorageAdapter → LegacyStorage
                    repo = New LegacyStorageAdapter() '' ← هذا هو Adapter & ← يتم اختيار نوع التنفيذ بناءً على mode

                Case Else
                    Throw New ArgumentException("نوع التخزين غير مدعوم")
            End Select
            '← يُحقن التنفيذ داخل التجريد (StudentService)
            Return New StudentService(repo) '' الحقن هنا يتم عبر IStudentRepository' التجريد يُحقن بالتنفيذ ← الجسر
        End Function
        'هنا تم تطبيق نمط
        '🧱 1. Repository Pattern ⇄ DIP + SRP
        '        Form1
        '└─ StudentController
        '    └─ StudentService
        '        └─ IStudentRepository
        '            ├─ CsvStudentRepository
        '            └─ LegacyStorageAdapter
        '        🎯 الوظيفة النظرية
        'DIP: تعتمد الطبقات العليا مثل StudentService على الواجهة IStudentRepository بدلًا من التنفيذ المباشر (مثل CsvStudentRepository)
        'SRP: كل طبقة تنفذ وظيفة واحدة فقط — التخزين لا يعرف المنطق، والمنطق لا يعرف التخزين
        '        ✅ النتيجة:
        'StudentService لا يعرف هل Create تحفظ في CSV أو Legacy
        'لأنه يتعامل فقط مع IStudentRepository ← هذا هو DIP

        'وهنا تم تطبيق نمط
        '🔄 5. Bridge Pattern ⇄ DIP + OCP
        'الـ StudentService هو التجريد ↪️
        'التنفيذ يُمرر عبر واجهة
        'IStudentRepository
        '← كذالك هو قابل للتبديل ↪️
        'أي تنفيذ جديد يتم إضافته دون تعديل StudentService
        '
        '



        'Public Shared Function Create() As IStudentService ' Repositoryترجع الدالة بنمط
        '    Dim repo = New CsvStudentRepository() 'يتم حقن تبعية Repository في الخدمة عن طريق واجهة، وهذه العملية جزء من منطق العمل وليس التخزين

        '    Dim service As New StudentService(repo) 'تم إنشاء الكائن الذي يحتوي المنطق ويعتمد على Repository

        '    ' إعادة الخدمة عبر الواجهة
        '    Return service
        'End Function
    End Class
End Namespace

