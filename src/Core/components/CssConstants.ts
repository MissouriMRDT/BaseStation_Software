import CSS from 'csstype';
import { getCurrentTheme } from './DarkMode';

export function Container(): CSS.Properties {
  let container: CSS.Properties;
  if (getCurrentTheme() === 'dark') {
    container = {
      display: 'flex',
      fontFamily: 'arial',
      borderTopWidth: '30px',
      borderColor: '#990000',
      borderBottomWidth: '2px',
      borderStyle: 'solid',
      flexWrap: 'wrap',
      flexDirection: 'column',
      padding: '5px',
      borderRadius: '5px',
      backgroundColor: '#333333',
      color: 'white',
    };
  } else {
    container = {
      display: 'flex',
      fontFamily: 'arial',
      borderTopWidth: '30px',
      borderColor: '#990000',
      borderBottomWidth: '2px',
      borderStyle: 'solid',
      flexWrap: 'wrap',
      flexDirection: 'column',
      padding: '5px',
      borderRadius: '5px',
    };
  }
  return container;
}

export function Button(): CSS.Properties {
  let button: CSS.Properties;
  if (getCurrentTheme() === 'dark') {
    button = {
      width: '100%',
      margin: '2.5px',
      fontSize: '14px',
      lineHeight: '24px',
      backgroundColor: '#404040',
      color: 'white',
      borderColor: '#990000',
      borderRadius: '5px',
    };
  } else {
    button = {
      width: '30%',
      margin: '2.5px',
      fontSize: '14px',
      lineHeight: '24px',
      borderRadius: '5px',
    };
  }
  return button;
}
// export default {
//   darkContainer,
//   lightContainer,
//   darkButton,
//   lightButton,
// };
