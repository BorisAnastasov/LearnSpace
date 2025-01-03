using LearnSpace.Core.Interfaces;
using LearnSpace.Core.Models.Submission;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static LearnSpace.Common.Constants;
namespace LearnSpace.Core.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly IRepository repository;
        public SubmissionService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            var submission = await repository.GetByIdAsync<Submission>(id);

            return submission != null;
        }
        public async Task CreateSubmissionAsync(string userId, int assignmentId, IFormFile file)
        {
            var student = await repository.GetStudentAsync(userId);
            var sub = student.Submissions.FirstOrDefault(s => s.AssignmentId == assignmentId);
            byte[] data;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                data = memoryStream.ToArray();
            }

            if (sub == null)
            {
                var submission = new Submission
                {
                    AssignmentId = assignmentId,
                    StudentId = student.Id,
                    FileName = Path.GetFileName(file.FileName),
                    FileType = file.ContentType,
                    SubmittedOn = DateTime.Now,
                    FileContent = data
                };

                await repository.AddAsync(submission);
            }
            else
            {
                sub.FileName = Path.GetFileName(file.FileName);
                sub.FileType = file.ContentType;
                sub.SubmittedOn = DateTime.Now;
                sub.FileContent = data;
            }
            await repository.SaveChangesAsync();

        }

        public async Task<SubmissionsViewModel> GetAllSubmissionsForAssignmentAsync(int assignmentId)
        {
            var assignment = await repository.GetByIdAsync<Assignment>(assignmentId);
            var submissions = assignment.Submissions
                                        .Select(s => new SubmissionQueryModel
                                        {
                                            Id = s.Id,
                                            AssignmentId = assignmentId,
                                            AssignmentTitle = assignment.Title,
                                            FileName = s.FileName,
                                            SubmittedOn = s.SubmittedOn.ToString(DateFormat),
                                            StudentName = s.Student.ApplicationUser.FirstName + " " + s.Student.ApplicationUser.LastName,

                                        }).ToList();
            var result = new SubmissionsViewModel
            {
                Submissions = submissions
            };
            return result;
        }

        public async Task<SubmissionFileModel> GetFileBySubmissionIdAsync(int submissionId)
        {
            var submission = await repository.GetByIdAsync<Submission>(submissionId);
            var model = new SubmissionFileModel
            {
                FileContent = submission.FileContent,
                FileName = submission.FileName,
                FileType = submission.FileType,
            };

            return model;
        }

        public async Task DeleteSubmissionIdAsync(int submissionId)
        {
            await repository.DeleteAsync<Submission>(submissionId);
            await repository.SaveChangesAsync();
        }

        public async Task<SubmissionsViewModel> GetAllSubmissionsForTeacherAsync(string userId)
        {
            var teacherId = repository.GetTeacherAsync(userId).Result.Id;
            var submissions = await repository
                                            .AllReadOnly<Submission>()
                                            .Where(s => s.Assignment.Course.TeacherId == teacherId)
                                            .Select(s => new SubmissionQueryModel
                                            {
                                                Id = s.Id,
                                                AssignmentId = s.AssignmentId,
                                                StudentName = s.Student.ApplicationUser.FirstName + " " + s.Student.ApplicationUser.LastName,
                                                AssignmentTitle = s.Assignment.Title,
                                                FileName = s.FileName,
                                                SubmittedOn = s.SubmittedOn.ToString(DateFormat)

                                            }).ToListAsync();
            var model = new SubmissionsViewModel
            {
                Submissions = submissions
            };

            return model;
        }

        public bool ContainsOnlyAllowedFileType(IFormFile file)
        {
            var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx", ".txt" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(fileExtension);
        }

        public bool SizeIsNotTooBig(IFormFile file)
        {
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB

            return file.Length <= maxFileSize;
            
        }

        public async Task<bool> AssignmentExistsByIdAsync(int id)
        {
            var assignment = await repository.GetByIdAsync<Assignment>(id);

            return assignment != null;
        }
    }
}
