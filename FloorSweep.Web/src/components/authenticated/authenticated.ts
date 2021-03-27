import { get as getTokenData } from '../../store/tokenData'
import { loginRedirect } from '../../services/authenticationService'

export const init = (async () => await ensureToken())()

async function ensureToken () {
  const tokenData = getTokenData()
  if (!tokenData.token) {
    loginRedirect()
  }
}
