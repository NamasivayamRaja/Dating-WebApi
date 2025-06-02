import { HttpParams, HttpResponse } from "@angular/common/http";
import { UserParams } from "../_models/user-params";
import { Pagination } from "../_models/pagination";
import { signal } from "@angular/core";
import { PaginatedResult } from "../_models/paginated-result";

export function setPaginatedResponse<T>(response: HttpResponse<T>,
    paginatedResultSignal: ReturnType<typeof signal<PaginatedResult<T> | null>>) {
    paginatedResultSignal.set({
        items: response.body as T,
        pagination: JSON.parse(response.headers.get('Pagination')!) as Pagination
    });
}

export function setPaginatedHeaderparams(pageSize: number, pageNumber: number): HttpParams {
    let params = new HttpParams();

    if (pageNumber && pageSize) {
        params = params.append('PageNumber', pageNumber);
        params = params.append('PageSize', pageSize);
    }
    return params;
}
