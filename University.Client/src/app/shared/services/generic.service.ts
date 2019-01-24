import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError, Observable } from 'rxjs';
import { NotFoundError } from '../base-model/notfound-error.model';
import { UnAuthorizedError } from '../base-model/unAuthorized-error.model';
import { BadRequestError } from '../base-model/badrequest-error.model';
import { UnknownError } from '../base-model/unknown-error.model';

@Injectable()
export class GenericService {
  url: string;
  resourceName: string;

  constructor(
    protected httpClient: HttpClient
  ) {}

  getEntityById<T>(id: number): Observable<T> {
    const urlAddress = this.url + `/${id}`;

    return this.httpClient
      .get<T>(urlAddress)
      .pipe(catchError(this.handleError));
  }

  getEntities<T>(): Observable<T[]> {
    return this.httpClient.get<T[]>(this.url).pipe(
      catchError(error => {
        return throwError(error);
      })
    );
  }

  addEntity<T>(entityToAdd: T): Observable<T> {
    return this.httpClient.post<T>(this.url , entityToAdd).pipe(
      catchError(error => {
        return throwError(error);
      })
    );
  }

  deleteEntity(id: number) {
    return this.httpClient.delete(this.url + `/${id}`).pipe(
      catchError(error => {
        return throwError(error);
      })
    );
  }

  updateEntity<T>(id: number, entity: T) {
    return this.httpClient.put(this.url + `/${id}`, entity).pipe(
      catchError(error => {
        return throwError(error);
      })
    );
  }

  private handleError(error: Response) {
    if (error.status === 404) {
      return Observable.throw(new NotFoundError(error, this.resourceName));
    }

    if (error.status === 401) {
      return Observable.throw(new UnAuthorizedError(error, this.resourceName));
    }

    if (error.status === 400) {
      return Observable.throw(new BadRequestError(error, this.resourceName));
    }

    return Observable.throw(new UnknownError(error));
  }
}
