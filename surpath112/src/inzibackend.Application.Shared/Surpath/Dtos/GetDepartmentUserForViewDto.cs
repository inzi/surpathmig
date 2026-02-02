namespace inzibackend.Surpath.Dtos
{
    public class GetDepartmentUserForViewDto
    {
        public DepartmentUserDto DepartmentUser { get; set; }

        public string UserName { get; set; }

        public string TenantDepartmentName { get; set; }

    }
}