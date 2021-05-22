import ConfigStore from "../store/configStore";
import type { Result } from "./fetchSafe";
import { fetchAuthenticated } from "./fetchSafe";
let baseUrl;

async function getBaseUrl(): Promise<string> {
  if (!baseUrl) {
    const configStore = ConfigStore.create();
    const config = await configStore.await();
    baseUrl = config.apiBaseUrl;
  }
  return baseUrl;
}

export async function get<T>(url: string): Promise<Result<T>> {
  const baseUrl = await getBaseUrl();
  return await fetchAuthenticated<T>(`${baseUrl}${url}`);
}

export async function post<T>(url: string, data: any): Promise<Result<T>> {
  const baseUrl = await getBaseUrl();
  return await fetchAuthenticated<T>(`${baseUrl}${url}`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
}

export async function put<T>(url: string, data: any): Promise<Result<T>> {
  const baseUrl = await getBaseUrl();
  return await fetchAuthenticated<T>(`${baseUrl}${url}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
}
