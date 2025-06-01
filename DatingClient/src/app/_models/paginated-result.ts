import { Pagination } from "./pagination";

export interface PaginatedResult<T>
{
    items?:T,
    pagination: Pagination
}