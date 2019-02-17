import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { BaseService } from './base.service';
import { LoggingService } from '../logging/logging.service';

export class BaseHttpService extends BaseService {

  constructor(protected httpService: HttpClient, loggingService: LoggingService) {
    super(loggingService);
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  protected handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      /** Log with the LoggingService */
      this.logError(`"${operation}" failed: ${error.message}`);

      if (result) {
        // Let the app keep running by returning an empty result.
        return of(result as T);
      }
      return error;
    };
  }
}
