namespace ClinicApp.Validation
{
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _MaxSizeInBytes;
        private readonly int _MaxSizeInMB;
        private readonly List<string> allowedExtensions=new List<string>();
        public FileSizeAttribute(int maxSizeInMB, string[] allowedExtensions) {
            _MaxSizeInBytes = maxSizeInMB * 1024 *1024;
            this._MaxSizeInMB = maxSizeInMB;
            foreach(var ex in allowedExtensions)
            {
                this.allowedExtensions.Add(ex.ToLower());
            
            }
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null) return ValidationResult.Success;

            if (value is IFormFile file) 
            {
                if (file.Length > _MaxSizeInBytes) return new ValidationResult($"Max File Size is {_MaxSizeInMB} MB");
            
                string ext = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(ext)) 
                    return new ValidationResult($"Allowed Extensions are {string.Join("     , " , allowedExtensions)}");
            }

            return ValidationResult.Success;
        }
    }
}
