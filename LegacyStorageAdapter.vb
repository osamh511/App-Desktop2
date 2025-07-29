Imports LAERN_USE.BLL.Interface
Imports LAERN_USE.DomainModels
Imports LAERN_USE.ExternalSources
''' <summary>
'''        🧠 🧠 ما يقوم به الـ Adapter فعليًا:مايقوم فية الـ Adapter هو
'''🧩 ربط الكائن الخارجي: مثل CsvStudentRepository أو SqlStudentRepository
'''🔄 تحويل الوظائف: مثل تحويل Load(id) إلى Read(id)
'''📦 تمكين التوافق: يسمح لـ StudentService باستخدام الكائن الخارجي كما لو أنه يطابق IStudentRepository
'''🛡️ عدم تعديل الملفات القديمة: كل شيء يتم داخل Adapter نفسه — الكود الأصلي يبقى كما هو
''' 🧠 إذًا ما هو دور الـ Adapter؟
''' 

'''الوظيفة	كيف تُنفذ
'''🧩 ربط الكائن الخارجي	عن طريق تكوين داخلي Private ReadOnly _externalSource
'''🔄 تحويل الوظائف	مثل تحويل Load(id) إلى Read(id)
'''📦 تمكين التوافق	يسمح لـ StudentService باستخدام الكائن الخارجي كما لو أنه يطابق IStudentRepository
'''🛡️ عدم تعديل الملفات القديمة	كل شيء يتم داخل Adapter نفسه — الكود الأصلي يبقى كما هو

Namespace BLL.Adapter 'يحوّل الواجهة إلى ما يتوافق مع IStudentRepository


    ''' <summary>
    ''' 📘 شرح بسيط للمحول (Adapter)
    ''' الهدف	تحويل واجهة غير متوافقة إلى واجهة متوافقة دون تعديل الكود الأصلي
'✅ المحول هو كلاس وسيط يسمح للنظام الحديث أن يتعامل مع كائن قديم أو غير متوافق، دون الحاجة لتعديل الكائن القديم نفسه.
    ''' </summary>
    Public Class LegacyStorageAdapter
        '✅ هذا الكلاس يحوّل
        'LegacyStudentStorage
        'إلى ما يتوافق مع النظام الجديد ✅
        'وينفذ فقط الوظائف المدعومة من النظام القديم
        Implements IStudentRepository

        Private ReadOnly _legacyStorage As New LegacyStudentStorage()
        ' هنا الخطوة الرابع Adapter
        '''        🔎 معنى هذا:
        '''المحول يترجم Read(id) إلى Load(id)
        '''الكود في StudentService و Form1 لا يعرف أن هذا تنفيذ "قديم"
        '''✔️ وهذا يحقق LSP ← لأن LegacyStorageAdapter يمكن استخدامه كـ IStudentRepository بشكل موثوق
        '''✔️ ويحقق OCP ← لأننا أضفنا محول جديد دون تعديل LegacyStudentStorage
        Public Function Read(id As Integer) As STUDENT4 Implements IStudentRepository.Read
            Return _legacyStorage.Load(id) ' Load(id) الى Read(id هنا يتم ترجمة 
        End Function

        Public Sub Create(student As STUDENT4) Implements IStudentRepository.Create
            Throw New NotSupportedException("Create غير مدعومة في التخزين القديم")
        End Sub

        Public Sub Update(student As STUDENT4) Implements IStudentRepository.Update
            Throw New NotSupportedException("Update غير مدعومة")
        End Sub

        Public Sub Delete(id As Integer) Implements IStudentRepository.Delete
            Throw New NotSupportedException("Delete غير مدعومة")
        End Sub

        ''' <summary>
        '''      هنا يطبق الخطوة الاولى للنمط 🔌 2. Adapter Pattern ⇄ OCP + LSP
        '''الهدف: جعل مصدر قديم غير متوافق يعمل داخل النظام الحديث دون تغييره
        '''     🔎 ماذا يحدث؟
        '''LegacyStorageAdapter
        '''يستدعي دالة خاصة بالنظام القديم
        '''Load(id)
        '''ثم يُعيد النتيجة داخل
        '''List 
        '''للتماشي مع النظام الحديث
        '''✔️ هذا التحويل يعكس OCP
        ''': لم نعدل النظام القديم ← فقط أضفنا محول للتوافق
        '''

        '''🧠 ملخص التدفق في Adapter
        '''Form1 → Factory → StudentService → LegacyStorageAdapter → LegacyStorage
        Public Function GetAll() As List(Of STUDENT4) Implements IStudentRepository.GetAll
            Return New List(Of STUDENT4) From {_legacyStorage.Load(1)}
        End Function
    End Class
End Namespace