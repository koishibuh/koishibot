﻿using Koishibot.Core.Features.Todoist.Models;

namespace Koishibot.Core.Features.Todoist.Interface;

public interface ITodoistService
{
	Task CreateTask(TodoistTaskDto taskDTO);
}