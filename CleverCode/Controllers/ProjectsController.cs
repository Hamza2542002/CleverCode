﻿using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Helpers.Error_Response;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CleverCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects()
        {
            var result = await _projectService.GetAllProjectsAsync();
            return Ok(new BaseResponse(HttpStatusCode.OK, result.Data, "Projects Fetched Successfulltu"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProjectById(int id)
        {
            var result = await _projectService.GetProjectByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound));
            }
            return Ok(new BaseResponse(HttpStatusCode.OK, result.Data, "Projects Fetched Successfulltu"));
        }

        [HttpGet("service/{service_id}")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjectsByServiceId(int service_id)
        {
            var result = await _projectService.GetSerivceProjects(service_id);
            if (!result.Success)
            {
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "No projects found for this service."));
            }
            return Ok(new BaseResponse(HttpStatusCode.OK, result.Data, "Projects Fetched Successfulltu"));
        }
        [Authorize(Roles = "Admin,super-Admin", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject([FromForm] ProjectDto projectDto)
        {
            var result = await _projectService.CreateProjectAsync(projectDto);
            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, result.Message));
            }
            return Ok(new BaseResponse(HttpStatusCode.OK, result.Data, result.Message));
        }
        [Authorize(Roles = "Admin,super-Admin", AuthenticationSchemes = "Bearer")]
        [HttpPost("services/{service_id}/{project_id}")]
        public async Task<ActionResult<ProjectDto>> AddProjectToService(int service_id, int project_id)

        {
            var result = await _projectService.AddProjectToService(service_id, project_id);
            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, result.Message));
            }
            return Ok(new BaseResponse(HttpStatusCode.OK, result.Data, "Project added to service successfully."));
        }
        [Authorize(Roles = "Admin,super-Admin", AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> UpdateProject(int id, ProjectDto projectDto)
        {
            if (id != projectDto.Project_ID)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest,"Miss match ID"));
            }

            var result = await _projectService.UpdateProjectAsync(id, projectDto);
            if (!result.Success)
            {
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound,result.Message));
            }

            return Ok(new BaseResponse(HttpStatusCode.OK, result.Data, "Projects Updated Successfulltu"));
        }

        [Authorize(Roles = "Admin,super-Admin", AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            return Ok(new BaseResponse(HttpStatusCode.OK, result.Data, "Projects Deleted Successfulltu"));
        }
        [Authorize(Roles =  "Admin,super-Admin", AuthenticationSchemes = "Bearer")]
        [HttpDelete("service/{service_id}/{project_id}")]
        public async Task<ActionResult<ProjectDto>> DeleteProjectFromService(int service_id, int project_id)
        {
            var result = await _projectService.DeleteProjectFromService(service_id, project_id);
            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, result.Message));
            }
            return Ok(new BaseResponse(HttpStatusCode.OK, result.Data, "Project Deleted From service successfully."));
        }
    }
}