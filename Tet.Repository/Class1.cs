namespace Tet.Repository;

public class Class1
{
    //ORM: Object-Realational Mapping
    
    // Code First và Database First
    
    //Thông thường để trên code có thể làm việc với database mình
    //cần ánh xa các table(mapping) từ database lên code ddeer dễ quản lý và làm việc
    
    
    //Databse First:
    //  -Làm việc với database có sẵn, setup 1 Database
    //  -Và tạo Create Table, Create Database, Setup Field, Set các Relationship
    //  -Sau đó ở trên code sử dụng các Driver hoặc Orm để kết nối xuống Database.
    //  -Trên code sẽ tạo những class ứng với các table trong dâtbase
    //  -Sd khi nào: khi mình có Database có sẵn và dử dụng trong rất nhiều năm r,
    //  những dự án duy trì(Maintain)
    
    //Code First:
    //  - Mình sẽ k setup Database thủ công bằng các Query
    //  - Không vô tạo Create Table, Create Database, Setup Field, Set các Relationship
    //  - Mình design Databse bằng những class trên code luôn, Trn code setup các entity, casc Field,
    //  - Sau đó ánh xạ từ các class xuong tabel trong Databse 
    //   Sử dụng khi: Dự án mới, hiện đại, nhiều công cụ hỗ trợ.
    
    // Vậy làm sao ánh xạ được, từ các class trên code xuống Database: ORM: Object-Realational Mapping
    
    //Tuyệt đỉnh ORM của .NET: Entity Framework. Không biết sử dụng và thành thục sẽ ...... .NET
    
}