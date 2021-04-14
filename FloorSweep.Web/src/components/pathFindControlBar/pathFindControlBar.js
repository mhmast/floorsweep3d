import ConfigStore from '../../store/configStore';
import { fetchAuthenticated } from '../../services/fetchSafe';
export async function findPathAsync() {
    const configStore = ConfigStore.create();
    const config = await configStore.await();
    const body = {
        start_x: 100, start_y: 100, end_x: 300, end_y: 300,
    };
    await fetchAuthenticated(`${config.apiBaseUrl}/path`, { method: 'POST', body: JSON.stringify(body), headers: { 'Content-Type': 'application/json' } });
}
//# sourceMappingURL=pathFindControlBar.js.map