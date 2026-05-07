Tạo entity
Setup mối quan hệ giữa các Entity
- 1 là để .Net tự generate ra
- 2 là config trong DbContext
Tạo file DbContext
Tạo Migration và Update
->Xong tầng Repository

To Interface cho mỗi Service
Định nghĩa các request đầu vào cần gì
Định nghĩa các response trả ra như thế nào
Tạo những method cho những interface đó
Tạo class Service để implement những method định nghĩa interface

Lên file Program.cs trong API để dki các Service vào DI

Tạo Controller cho Service đã tạo
Định nghĩa các Endpoint cho Controller
Định nghĩa các Reuqest đầu vào sẽ cần gì
-   Có phải bo dy hay không
-   Có query hay không
-   Có path | route paremeter hay không
Authen này có cần Authen hay Author không

