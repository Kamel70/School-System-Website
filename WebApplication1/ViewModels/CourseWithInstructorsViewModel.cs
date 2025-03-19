using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class CourseWithInstructorsViewModel
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2,ErrorMessage ="The Name must be Greater Than 2 Character")]
        [MaxLength(10,ErrorMessage = "The Name must be Less Than 10 Character")]
        [Remote(action:"uniqueName",controller:"Course",ErrorMessage ="This Course is Already Exist")]
        public string Name { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "The Topic must be Greater Than 2 Character")]
        [MaxLength(10, ErrorMessage = "The Topic must be Less Than 10 Character")]
        public string Topic { get; set; }
        [Required]
        [Range(1,100, ErrorMessage = "The Min Degree Must be from 1 to 100")]
        public int? minDigree { get; set; }
        [Required]
        [Range (1,100,ErrorMessage ="The Total Degree Must be from 1 to 100")]
        [Remote(action:"CheckTotalDegree",controller:"Course",AdditionalFields = "minDigree", ErrorMessage ="The Total Degree Must Be Greater than Min Degree")]
        public int? TotalDigree { get; set; }
        [Display(Name="Instructor")]
        [Required]
        public int InsID { get; set; }
        public List<Instructor> instructors { get; set; }=new List<Instructor>();
    }
}
