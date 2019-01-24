import { CourseStudentForCreateModel } from './course-student-for-create.model';

export class CourseForCreateModel {
    name: string;
    teacherName: string;
    location: string;
    students: CourseStudentForCreateModel[];
}
