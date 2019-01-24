import { CourseStudentForUpdateModel } from './course-student-for-update.model';

export class CourseForUpdateModel {
    name: string;
    teacherName: string;
    location: string;
    students: CourseStudentForUpdateModel[];
}
