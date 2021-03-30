import { loginRedirect } from './authenticationService';

export interface Result<T>
{
    data:T,
    error:string,
    statusCode:number
}
export default async function fetchSafe<T>(input:RequestInfo, init?:RequestInit)
: Promise<Result<T>> {
  try {
    const response = await fetch(input, init);
    if (response.ok) {
      const data = await response.json() as T;
      return { data, statusCode: response.status, error: null };
    }
    if (response.status === 401 || response.status === 403) {
      loginRedirect();
      return { data: null, statusCode: response.status, error: response.statusText };
    }
    return { data: null, statusCode: response.status, error: response.statusText };
  } catch (e) {
    return { data: null, statusCode: 500, error: e };
  }
}

export function formEncode(details:any):URLSearchParams {
  return new URLSearchParams(details);
}
