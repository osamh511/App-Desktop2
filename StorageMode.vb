'↪️ وهكذا يصبح لديك وحدة مستقلة اسمها StorageMode يمكن استخدامها في:
'الفورم UI عند اختيار نوع التخزين
'الـ Factory عند إنشاء StudentService
'في الاختبارات لو أردت تحديد نوع التخزين
Namespace BLL.Enums
    Public Enum StorageMode 'وضع التحزين
        Csv
        Legacy

    End Enum

End Namespace
