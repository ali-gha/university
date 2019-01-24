import { CourseStudentModel } from './course-student.model';

export class CourseModel {
    id: number;
    name: string;
    teacherName: string;
    location: string;
    students: CourseStudentModel[];
}
