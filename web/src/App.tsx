import { ConfigProvider, theme } from 'antd';
import { EditorLayout } from './components/EditorLayout';
import { SettingsBar } from './components/SettingsBar';

function App() {
  return (
    <ConfigProvider
      theme={{
        algorithm: theme.darkAlgorithm,
      }}
    >
      <SettingsBar />
      <EditorLayout />
    </ConfigProvider>
  );
}

export default App;
