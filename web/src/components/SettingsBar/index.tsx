import { Button, Select } from 'antd';
import { StyledContainer, StyledContainerItem } from './styles';

export const SettingsBar = () => {
  return (
    <StyledContainer>
      <StyledContainerItem></StyledContainerItem>
      <StyledContainerItem>
        <Select
          defaultValue="release"
          options={[
            { value: 'release', label: 'Release' },
            { value: 'debug', label: 'Debug' },
          ]}
        />
      </StyledContainerItem>
    </StyledContainer>
  );
};
